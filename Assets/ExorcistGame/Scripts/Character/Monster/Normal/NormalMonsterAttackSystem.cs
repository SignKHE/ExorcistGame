using ExorcistGame.Character.State;
using ExorcistGame.Seeker;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character.Monster.Normal
{
    [BurstCompile]
    public partial struct NormalMonsterAttackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);

            foreach (var (attackData, movementData, transform, foundEntityBuffers, entity)
                     in SystemAPI.Query<RefRW<AttackData>, RefRW<MovementData>, RefRO<LocalTransform>, DynamicBuffer<FoundEntityBuffer>>()
                         .WithAll<MonsterData, AttackState>().WithEntityAccess())
            {
                // 만약 탐색된 타겟이 없다면 공격 종료
                if (foundEntityBuffers.IsEmpty)
                {
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                    continue;
                }
                
                if(! transformLookup.TryGetComponent(foundEntityBuffers[0].Value, out LocalTransform targetTransform)) continue;
                float3 position = transform.ValueRO.Position;
                float3 targetPosition = targetTransform.Position;
                float distanceSq = math.distancesq(position, targetPosition);
                float attackRangeSq = attackData.ValueRO.AttackRange * attackData.ValueRO.AttackRange;
                
                // 공격 범위 밖이라면 공격 종료
                if (distanceSq > attackRangeSq)
                {
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                    continue;
                }
                
                movementData.ValueRW.Direction = math.normalizesafe(targetPosition - position) * 0.00001f;
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}