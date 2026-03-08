using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character.Spawn
{
    [BurstCompile]
    public partial struct SpawnInitializeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 스폰 정보 싱글톤 엔티티 가져오기 (없다면 시스템 종료)
            if (!SystemAPI.TryGetSingleton<SpawnConfig>(out SpawnConfig config)) return;
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (spawnPoolData, spawnPoolBuffer, spawnPoolEntity) 
                     in SystemAPI.Query<RefRO<SpawnPoolData>, DynamicBuffer<SpawnPoolBuffer>>().WithEntityAccess())
            {
                // 스폰 풀이 비어있을때만 초기화 진행
                if(!spawnPoolBuffer.IsEmpty) continue;
                
                NativeArray<Entity> instances = new ((int)math.round(config.SpawnMax * 1.1), Allocator.Persistent);

                ecb.Instantiate(spawnPoolData.ValueRO.MonsterPrefab, instances);

                foreach (Entity instance in instances)
                {
                    ecb.AddComponent(instance, LocalTransform.FromPosition(new float3(0,-200,0)));
                    ecb.AddComponent(instance, new Disabled());
                    
                    ecb.AppendToBuffer(spawnPoolEntity, new SpawnPoolBuffer() {LoadedEntity = instance});
                }
                
                instances.Dispose();
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}