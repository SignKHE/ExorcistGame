using ExorcistGame.VisualSync;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Spawn
{
    [BurstCompile]
    public partial struct SpawnProcessSystem : ISystem
    {
        private Random _random;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            _random = new Unity.Mathematics.Random(39);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 플레이어 태그를 가진 플레이어 엔티티 가져오기 (없다면 시스템 종료)
            if(!SystemAPI.TryGetSingletonEntity<PlayerTag>(out var playerEntity)) return;
            // 스폰 정보 싱글톤 엔티티 가져오기 (없다면 시스템 종료)
            if (!SystemAPI.TryGetSingleton<SpawnConfig>(out SpawnConfig config)) return;

            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            // 플레이어 위치 정보 가져오기
            float3 playerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
            
            float angle = _random.NextFloat(min:0.0f,max:1.0f) * math.PI * 2.0f;
            math.sincos(angle, out float sin, out float cos);
            float3 offset = new float3(cos * config.Radius, 0, sin * config.Radius);
            float3 spawnPosition = playerPosition + offset;

            foreach (var (request, entity) in SystemAPI.Query<RefRO<SpawnRequestData>>().WithEntityAccess())
            {
                for (int i = 0; i < request.ValueRO.Count; i++)
                {
                    Entity newMonster = ecb.Instantiate(config.MonsterPrefab);
                    float3 randomOffset = _random.NextFloat3Direction() * _random.NextFloat(0, request.ValueRO.Radius);
                    randomOffset.y = 0;
                    float3 finalPos = spawnPosition + randomOffset;
                    ecb.AddComponent(newMonster, LocalTransform.FromPosition(finalPos));
                    ecb.AddComponent(newMonster, new VisualSyncTag());
                }
                ecb.DestroyEntity(entity);
            }
        }
    }
}