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
        public NativeQueue<Entity> ProjectilePool;
        
        private bool _isInitialized;
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            
            ProjectilePool = new NativeQueue<Entity>(Allocator.Persistent);
            _isInitialized = false;
            
            state.RequireForUpdate<ProjectileConfig>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(!SystemAPI.TryGetSingleton<ProjectileConfig>(out var config)) return;
            
            // 아직 오브젝트 풀 초기화를 하지 않았다면 초기화
            if (!_isInitialized)
            {
                int poolSize = 1000;
                
                NativeArray<Entity> instances = new NativeArray<Entity>(poolSize, Allocator.Persistent);
                
                state.EntityManager.Instantiate(config.ProjectilePrefab, instances);

                for (int i = 0; i < poolSize; i++)
                {
                    SystemAPI.SetComponent(instances[i],LocalTransform.FromPosition(new float3(0f,-100,0f)));
                    SystemAPI.SetComponentEnabled<ProjectileData>(instances[i], false);
                    ProjectilePool.Enqueue(instances[i]);
                }
                
                instances.Dispose();
                _isInitialized = true;
                return;
            }
            
            foreach (var (spawnBuffer, spawnerEntity) in SystemAPI.Query<DynamicBuffer<ProjectileSpawnBuffer>>().WithEntityAccess())
            {
                if (spawnBuffer.IsEmpty) continue;

                foreach (var request in spawnBuffer)
                {
                    if (ProjectilePool.TryDequeue(out Entity pooledProjectile))
                    {
                        SystemAPI.SetComponent(pooledProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        SystemAPI.SetComponent(pooledProjectile, request.Data);
                        SystemAPI.SetComponentEnabled<ProjectileData>(pooledProjectile, true);

                    }
                    else
                    {
                        Entity newProjectile = state.EntityManager.Instantiate(config.ProjectilePrefab);
                        SystemAPI.SetComponent(newProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        SystemAPI.SetComponent(newProjectile, request.Data);
                        SystemAPI.SetComponentEnabled<ProjectileData>(newProjectile, true);
                    }
                }
            
                // 처리 끝난 버퍼 비우기
                spawnBuffer.Clear();
            }
        }

        public void OnDestroy(ref SystemState state)
        {
            if(ProjectilePool.IsCreated) ProjectilePool.Dispose();
        }
    }
}