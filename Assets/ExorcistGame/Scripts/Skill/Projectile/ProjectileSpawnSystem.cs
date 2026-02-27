using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [BurstCompile]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (spawner, entity) in SystemAPI.Query<RefRW<ProjectileSpawner>>().WithEntityAccess())
            {
                // 아직 오브젝트 풀 초기화를 하지 않았다면 초기화
                if (!spawner.ValueRO.IsInitialized)
                {
                    NativeArray<Entity> instances = new NativeArray<Entity>(spawner.ValueRO.PoolSize, Allocator.Persistent);
                
                    state.EntityManager.Instantiate(spawner.ValueRO.ProjectilePrefab, instances);
                    
                    DynamicBuffer<ProjectileSpawnPoolBuffer> poolBuffer = SystemAPI.GetBuffer<ProjectileSpawnPoolBuffer>(entity);

                    for (int i = 0; i < spawner.ValueRO.PoolSize; i++)
                    {
                        SystemAPI.SetComponent(instances[i],LocalTransform.FromPosition(new float3(0f,-100,0f)));
                        SystemAPI.SetComponentEnabled<ProjectileData>(instances[i], false);
                        poolBuffer.Add(new ProjectileSpawnPoolBuffer { ProjectileEntity = instances[i] });
                    }
                
                    instances.Dispose();
                    spawner.ValueRW.IsInitialized = true;
                    return;
                }
            }
            
            
            
            foreach (var (spawner, spawnBuffer, spawnPoolBuffers, spawnerEntity) in SystemAPI.Query<RefRW<ProjectileSpawner>, DynamicBuffer<ProjectileSpawnRequestBuffer>, DynamicBuffer<ProjectileSpawnPoolBuffer>>().WithEntityAccess())
            {
                if (spawnBuffer.IsEmpty) continue;

                foreach (var request in spawnBuffer)
                {
                    if (spawnPoolBuffers.IsEmpty)
                    {
                        Entity newProjectile = state.EntityManager.Instantiate(spawner.ValueRO.ProjectilePrefab);
                        SystemAPI.SetComponent(newProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        SystemAPI.SetComponent(newProjectile, request.Data);
                        SystemAPI.SetComponentEnabled<ProjectileData>(newProjectile, true);
                    }
                    else
                    {
                        int lastIndex = spawnPoolBuffers.Length - 1;
                        Entity pooledProjectile = spawnPoolBuffers[lastIndex].ProjectileEntity; // 맨 끝에 있는 걸 꺼내고
                        spawnPoolBuffers.RemoveAt(lastIndex); // 버퍼에서 지웁니다.
                    
                        SystemAPI.SetComponent(pooledProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        SystemAPI.SetComponent(pooledProjectile, request.Data);
                        SystemAPI.SetComponentEnabled<ProjectileData>(pooledProjectile, true);
                    }
                }
            
                // 처리 끝난 버퍼 비우기
                spawnBuffer.Clear();
            }
        }
    }
}