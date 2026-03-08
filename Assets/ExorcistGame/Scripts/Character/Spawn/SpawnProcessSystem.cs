using ExorcistGame.Character.Monster;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character.Spawn
{
    [BurstCompile]
    public partial struct SpawnProcessSystem : ISystem
    {
        private Random _random;
        /// <summary>
        /// 몬스터 쿼리
        /// </summary>
        private EntityQuery _monsterQuery;

        private EntityQuery _playerQuery;
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            uint seed = (uint)System.DateTime.Now.Ticks + 39;
            _random = new Unity.Mathematics.Random( seed: seed);
            _monsterQuery = state.GetEntityQuery(ComponentType.ReadOnly<MonsterData>());
            _playerQuery = SystemAPI.QueryBuilder().WithAll<PlayerTag, CharacterTag>().Build();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 스폰 정보 싱글톤 엔티티 가져오기 (없다면 시스템 종료)
            if (!SystemAPI.TryGetSingleton<SpawnConfig>(out SpawnConfig config)) return;
            
            // 플레이어 태그를 가진 캐릭터 엔티티를 가져오고 위치정보 가져오기 (없다면 시스템 종료)
            if(! (_playerQuery.CalculateEntityCount() > 0) ) return;
            
            float3 playerPosition = float3.zero;
            foreach (var playerTransform 
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag,CharacterTag>())
            {
                playerPosition = playerTransform.ValueRO.Position;
                break;
            }
            
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (poolData, poolDataBuffer, requestDataBuffer, entity) 
                     in SystemAPI.Query<RefRO<SpawnPoolData>, DynamicBuffer<SpawnPoolBuffer>, DynamicBuffer<SpawnRequestData>>().WithEntityAccess())
            {
                if(requestDataBuffer.IsEmpty) continue;
                
                foreach (var request in requestDataBuffer)
                {
                    // 현재 몬스터 갯수 가져오기
                    int currentMonsterCount = _monsterQuery.CalculateEntityCount();
                    // 현재 스폰 가능한 몬스터 갯수가 남아있지 않다면 스폰 로직 종료
                    if (currentMonsterCount >= config.SpawnMax) break;
                    // 남은 스폰 갯수 저장
                    int leftSpawnCount = config.SpawnMax - currentMonsterCount;
                
                    int spawnCount = math.min(request.Count, leftSpawnCount);
                
                    float angle = _random.NextFloat(min:0.0f,max:1.0f) * math.PI * 2.0f;
                    math.sincos(angle, out float sin, out float cos);
                    float3 offset = new float3(cos * config.Radius, 0, sin * config.Radius);
                    float3 spawnPosition = playerPosition + offset;
                
                    for (int i = 0; i < spawnCount; i++)
                    {
                        Entity newMonster;
                        if (poolDataBuffer.IsEmpty)
                        {
                            newMonster = ecb.Instantiate(poolData.ValueRO.MonsterPrefab);
                        }
                        else
                        {
                            int lastIndex = poolDataBuffer.Length - 1;
                            newMonster = poolDataBuffer[lastIndex].LoadedEntity; // 맨 끝에 있는 걸 꺼내고
                            poolDataBuffer.RemoveAt(lastIndex); // 버퍼에서 지웁니다.
                            ecb.RemoveComponent<Disabled>(newMonster);
                        }
                        ecb.AddComponent(newMonster, new SpawnedData() { SpawnPool = entity});
                        
                        float3 randomOffset = _random.NextFloat3Direction() * _random.NextFloat(0, request.Radius);
                        randomOffset.y = 0;
                        float3 finalPos = spawnPosition + randomOffset;
                        ecb.AddComponent(newMonster, LocalTransform.FromPosition(finalPos));
                    }
                }
                requestDataBuffer.Clear();
            }
            
            
        }
    }
}