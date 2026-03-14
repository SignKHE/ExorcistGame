using System.Collections.Generic;
using ExorcistGame.Character;
using ExorcistGame.Character.Monster;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Seeker
{
    [BurstCompile]
    public partial struct SeekingSystem : ISystem
    {
        /// <summary>
        /// 타겟대상 쿼리
        /// </summary>
        private EntityQuery _targetQuery;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _targetQuery = SystemAPI.QueryBuilder()
                .WithAll<CharacterTag, LocalTransform>()
                .Build();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var targetEntities = _targetQuery.ToEntityArray(state.WorldUpdateAllocator);
            var targetTransforms = _targetQuery.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);

            var job = new SeekAndSortJob
            {
                TargetEntities = targetEntities,
                TargetTransforms = targetTransforms,
                MonsterLookup = SystemAPI.GetComponentLookup<MonsterData>(true),
                PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true)
            };
        
            job.ScheduleParallel();
        }
    }
    
    // 정렬용 임시 데이터
    public struct DistanceData
    {
        public Entity Entity;
        public float DistanceSq;
    }

    // 거리 가까운 순서로 정렬하기 위한 Comparer
    public struct DistanceComparer : IComparer<DistanceData>
    {
        public int Compare(DistanceData x, DistanceData y)
        {
            return x.DistanceSq.CompareTo(y.DistanceSq);
        }
    }
    
    [BurstCompile]
    public partial struct SeekAndSortJob : IJobEntity
    {
        [ReadOnly] public NativeArray<Entity> TargetEntities;
        [ReadOnly] public NativeArray<LocalTransform> TargetTransforms;
        
        [ReadOnly] public ComponentLookup<MonsterData> MonsterLookup;
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;

        public void Execute(Entity seekerEntity, in LocalTransform seekerTransform, in SeekerData seekerData, ref DynamicBuffer<FoundEntityBuffer> foundBuffer)
        {
            foundBuffer.Clear();

            NativeList<DistanceData> tempResults = new NativeList<DistanceData>(Allocator.Temp);

            float rangeSq = seekerData.Range * seekerData.Range;
            float3 seekerPos = seekerTransform.Position;

            for (int i = 0; i < TargetEntities.Length; i++)
            {
                if (seekerEntity == TargetEntities[i]) continue;
                
                // 현재 타겟 엔티티가 시커가 대상으로 삼을 수 있는지 체크
                if ( ! (seekerData.TargetType switch
                    {
                        ETargetType.Player => PlayerLookup.HasComponent(TargetEntities[i]),
                        ETargetType.Monster => MonsterLookup.HasComponent(TargetEntities[i]),
                        _ => false
                    }) ) continue;

                float distSq = math.distancesq(seekerPos, TargetTransforms[i].Position);
            
                if (distSq <= rangeSq)
                {
                    tempResults.Add(new DistanceData 
                    { 
                        Entity = TargetEntities[i], 
                        DistanceSq = distSq 
                    });
                }
            }

            tempResults.Sort(new DistanceComparer());

            foreach (var result in tempResults)
            {
                foundBuffer.Add(new FoundEntityBuffer { Value = result.Entity });
            }

            tempResults.Dispose();
        }
    }
}