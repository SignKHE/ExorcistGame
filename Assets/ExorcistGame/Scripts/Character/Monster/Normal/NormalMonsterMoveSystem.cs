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
    public partial struct NormalMonsterMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);

            foreach (var (attackData, movementData, transform, foundEntityBuffers, entity)
                     in SystemAPI.Query<RefRO<AttackData>, RefRW<MovementData>, RefRO<LocalTransform>, DynamicBuffer<FoundEntityBuffer>>()
                         .WithAll<MonsterData, MoveState>().WithEntityAccess())
            {
                // 만약 탐색된 타겟이 없다면 움직임 종료
                if (foundEntityBuffers.IsEmpty)
                {
                    ecb.SetComponentEnabled<MoveState>(entity, false);
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                    continue;
                }
                
                if(! transformLookup.TryGetComponent(foundEntityBuffers[0].Value, out LocalTransform targetTransform)) continue;
                float3 position = transform.ValueRO.Position;
                float3 targetPosition = targetTransform.Position;
                float distanceSq = math.distancesq(position, targetPosition);
                float attackRangeSq = attackData.ValueRO.AttackRange * attackData.ValueRO.AttackRange;

                // 타겟과의 거리가 공격 범위보다 가까울 경우
                if (distanceSq < attackRangeSq)
                {
                    ecb.SetComponentEnabled<MoveState>(entity, false);
                    ecb.SetComponentEnabled<AttackState>(entity, true);
                    continue;
                }
                
                movementData.ValueRW.Direction = math.normalizesafe(targetPosition - position);
                
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}