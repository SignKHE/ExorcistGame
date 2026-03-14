using ExorcistGame.Character.Monster;
using ExorcistGame.Character.State;
using ExorcistGame.Seeker;
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
            
            foreach (var (attackData,movementData ,transform, foundEntityBuffers, entity) 
                     in SystemAPI.Query<RefRW<AttackData>, RefRO<MovementData>, RefRO<LocalTransform>, DynamicBuffer<FoundEntityBuffer>>().WithAll<PlayerTag,IdleState>().WithEntityAccess())
            {
                float3 movement = movementData.ValueRO.Direction;
                float distance = 0f;
                
                // 만약 움직임이 있다면 Move 상태로 변경
                if (math.lengthsq(movement) > float.Epsilon)
                {
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<IdleState>(entity, false);
                    ecb.SetComponentEnabled<MoveState>(entity, true);
                    
                    continue;
                }

                if (!foundEntityBuffers.IsEmpty)
                {
                    attackData.ValueRW.Target = foundEntityBuffers[0].Value;
                    
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