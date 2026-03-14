using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [BurstCompile]
    public partial struct InitializeProjectileSpawnerSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (spawner, entity) in SystemAPI.Query<RefRW<ProjectileSpawner>>().WithEntityAccess())
            {
                // 아직 오브젝트 풀 초기화를 하지 않았다면 초기화
                if (spawner.ValueRO.IsInitialized) continue;

                bool isPlayer = SystemAPI.HasComponent<PlayerTag>(entity);
                
                ecb.AddBuffer<ProjectileSpawnRequestBuffer>(entity);
                ecb.AddBuffer<ProjectileSpawnPoolBuffer>(entity);
                
                NativeArray<Entity> instances = new (spawner.ValueRO.PoolSize, Allocator.Persistent);
                
                ecb.Instantiate(spawner.ValueRO.ProjectilePrefab, instances);

                for (int i = 0; i < spawner.ValueRO.PoolSize; i++)
                {
                    ecb.AddComponent(instances[i],LocalTransform.FromPosition(new float3(0f,-100,0f)));
                    ecb.AddComponent(instances[i], spawner.ValueRO.ProjectileDataPreset);
                    ecb.AddComponent(instances[i], new MovementData() {Direction = float3.zero, Speed = spawner.ValueRO.ProjectileSpeed});
                    ecb.SetComponentEnabled<ProjectileData>(instances[i], false);
                    if (isPlayer) ecb.AddComponent(instances[i], new PlayerTag());
                    ecb.AppendToBuffer(entity, new ProjectileSpawnPoolBuffer { ProjectileEntity = instances[i] });
                    ecb.AddComponent(instances[i], new Disabled());
                }
                
                instances.Dispose();
                spawner.ValueRW.IsInitialized = true;
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}