using System.Linq;
using ExorcistGame.Character;
using ExorcistGame.Damage;
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
        /// <summary>
        /// 몬스터 쿼리
        /// </summary>
        private EntityQuery _monsterQuery;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            uint seed = (uint)System.DateTime.Now.Ticks + 39;
            _random = new Unity.Mathematics.Random( seed: seed);
            _monsterQuery = state.GetEntityQuery(ComponentType.ReadOnly<MonsterData>());
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 플레이어 태그를 가진 캐릭터 엔티티를 가져오고 위치정보 가져오기 (없다면 시스템 종료)
            if(! (SystemAPI.QueryBuilder().WithAll<PlayerTag, CharacterTag>().Build().CalculateEntityCount() > 0) ) return;
            
            float3 playerPosition = float3.zero;
            foreach (var playerTransform 
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag,CharacterTag>())
            {
                playerPosition = playerTransform.ValueRO.Position;
                break;
            }
            
            // 스폰 정보 싱글톤 엔티티 가져오기 (없다면 시스템 종료)
            if (!SystemAPI.TryGetSingleton<SpawnConfig>(out SpawnConfig config)) return;

            // 현재 몬스터 갯수 가져오기
            int currentMonsterCount = _monsterQuery.CalculateEntityCount();
            // 현재 스폰 가능한 몬스터 갯수가 남아있지 않다면 스폰 로직 종료
            if(currentMonsterCount >= config.SpawnMax)
            {
                //UnityEngine.Debug.Log($"Spawn Max: {config.SpawnMax} Current Monster Count: {currentMonsterCount}");
                return;
            }
            // 남은 스폰 갯수 저장
            int leftSpawnCount = config.SpawnMax - currentMonsterCount;

            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            
            float angle = _random.NextFloat(min:0.0f,max:1.0f) * math.PI * 2.0f;
            math.sincos(angle, out float sin, out float cos);
            float3 offset = new float3(cos * config.Radius, 0, sin * config.Radius);
            float3 spawnPosition = playerPosition + offset;

            foreach (var (request, entity) in SystemAPI.Query<RefRO<SpawnRequestData>>().WithEntityAccess())
            {
                // 요청받은 수보다 생성 가능한 몬스터 수가 적으면 그만큼만 생성
                int spawnCount = math.min(request.ValueRO.Count, leftSpawnCount);
                for (int i = 0; i < spawnCount; i++)
                {
                    //UnityEngine.Debug.Log($"몬스터 엔티티 스폰");
                    Entity newMonster = ecb.Instantiate(config.MonsterPrefab);
                    float3 randomOffset = _random.NextFloat3Direction() * _random.NextFloat(0, request.ValueRO.Radius);
                    randomOffset.y = 0;
                    float3 finalPos = spawnPosition + randomOffset;
                    ecb.AddComponent(newMonster, LocalTransform.FromPosition(finalPos));
                    ecb.AddComponent(newMonster, new VisualSyncTag());
                    ecb.AddComponent(newMonster, new MonsterData());
                    ecb.AddComponent(newMonster, new CharacterTag());
                    ecb.AddComponent(newMonster, new HPData() { MaxHP = 100f, HP = 100f});
                }
                ecb.DestroyEntity(entity);
            }
        }
    }
}