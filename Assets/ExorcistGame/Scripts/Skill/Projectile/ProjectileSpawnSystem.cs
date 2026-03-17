using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 투사체 스폰 시스템
    /// </summary>
    [BurstCompile]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (spawner, requestBuffer, spawnPoolBuffers, spawnerEntity) 
                     in SystemAPI.Query<RefRO<ProjectileSpawner>, DynamicBuffer<ProjectileSpawnRequestBuffer>, DynamicBuffer<ProjectileSpawnPoolBuffer>>().WithEntityAccess())
            {
                if (requestBuffer.IsEmpty) continue;

                foreach (var request in requestBuffer)
                {
                    if (spawnPoolBuffers.IsEmpty)
                    {
                        Entity newProjectile = ecb.Instantiate(spawner.ValueRO.ProjectilePrefab);
                        ecb.AddComponent(newProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        ecb.AddComponent(newProjectile, spawner.ValueRO.ProjectileDataPreset);
                        ecb.AddComponent(newProjectile, new MovementData() {Direction = request.Direction, Speed = spawner.ValueRO.ProjectileSpeed});
                        ecb.SetComponentEnabled<ProjectileData>(newProjectile, true);
                    }
                    else
                    {
                        int lastIndex = spawnPoolBuffers.Length - 1;
                        Entity pooledProjectile = spawnPoolBuffers[lastIndex].ProjectileEntity; // 맨 끝에 있는 걸 꺼내고
                        spawnPoolBuffers.RemoveAt(lastIndex); // 버퍼에서 지웁니다.
                    
                        ecb.AddComponent(pooledProjectile, LocalTransform.FromPosition(request.SpawnLocation));
                        ecb.AddComponent(pooledProjectile, spawner.ValueRO.ProjectileDataPreset);
                        ecb.AddComponent(pooledProjectile, new MovementData() {Direction = request.Direction, Speed = spawner.ValueRO.ProjectileSpeed});
                        ecb.SetComponentEnabled<ProjectileData>(pooledProjectile, true);
                        ecb.RemoveComponent<Disabled>(pooledProjectile);
                    }
                }
            
                // 처리 끝난 버퍼 비우기
                requestBuffer.Clear();
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}