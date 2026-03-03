using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [BurstCompile]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (spawner, spawnBuffer, spawnPoolBuffers, spawnerEntity) 
                     in SystemAPI.Query<RefRW<ProjectileSpawner>, DynamicBuffer<ProjectileSpawnRequestBuffer>, DynamicBuffer<ProjectileSpawnPoolBuffer>>().WithEntityAccess())
            {
                if (!spawner.ValueRO.IsInitialized) continue;
                if (spawnBuffer.IsEmpty) continue;

                foreach (var request in spawnBuffer)
                {
                    if (spawnPoolBuffers.IsEmpty)
                    {
                        Entity newProjectile = ecb.Instantiate(spawner.ValueRO.ProjectilePrefab);
                        ecb.AddComponent(newProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        ecb.AddComponent(newProjectile, request.Data);
                        ecb.SetComponentEnabled<ProjectileData>(newProjectile, true);
                    }
                    else
                    {
                        int lastIndex = spawnPoolBuffers.Length - 1;
                        Entity pooledProjectile = spawnPoolBuffers[lastIndex].ProjectileEntity; // 맨 끝에 있는 걸 꺼내고
                        spawnPoolBuffers.RemoveAt(lastIndex); // 버퍼에서 지웁니다.
                    
                        ecb.AddComponent(pooledProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        ecb.AddComponent(pooledProjectile, request.Data);
                        ecb.SetComponentEnabled<ProjectileData>(pooledProjectile, true);
                        ecb.RemoveComponent<Disabled>(pooledProjectile);
                    }
                }
            
                // 처리 끝난 버퍼 비우기
                spawnBuffer.Clear();
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}