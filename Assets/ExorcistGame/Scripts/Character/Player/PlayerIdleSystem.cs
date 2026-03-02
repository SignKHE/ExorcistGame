using ExorcistGame.Character.Monster;
using ExorcistGame.Character.State;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character.Player
{
    [BurstCompile]
    public partial struct PlayerIdleSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (attackData,movementData ,transform, entity) 
                     in SystemAPI.Query<RefRW<AttackData>, RefRO<MovementData>, RefRO<LocalTransform>>().WithAll<PlayerTag,IdleState>().WithEntityAccess())
            {
                // 근처의 적 유닛 찾기
                Entity closestTarget = Entity.Null;
                float minDistance = attackData.ValueRO.DetectionRange;
                float3 playerPos = transform.ValueRO.Position;
                float2 movement = movementData.ValueRO.MoveDirection;
                float distance = 0f;
                
                // 만약 움직임이 있다면 Move 상태로 변경
                if (math.lengthsq(movement) > float.Epsilon)
                {
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<IdleState>(entity, false);
                    ecb.SetComponentEnabled<MoveState>(entity, true);
                    
                    continue;
                }
                
                foreach (var (monsterTransform, monsterEntity) 
                         in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<MonsterData>().WithEntityAccess())
                {
                    distance = math.distancesq(playerPos, monsterTransform.ValueRO.Position);

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestTarget = monsterEntity;
                    }
                }

                // 가까운 적이 있다면 타겟으로 설정
                if (closestTarget != Entity.Null)
                {
                    attackData.ValueRW.Target = closestTarget;
                    
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<IdleState>(entity, false);
                    ecb.SetComponentEnabled<AttackState>(entity, true);
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}