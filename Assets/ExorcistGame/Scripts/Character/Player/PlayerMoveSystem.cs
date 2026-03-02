using ExorcistGame.Character.State;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character.Player
{
    [BurstCompile]
    public partial struct PlayerMoveSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (attackData, movementData, transform, entity)
                     in SystemAPI.Query<RefRW<AttackData>, RefRO<MovementData>, RefRO<LocalTransform>>().WithAll<PlayerTag,MoveState>().WithEntityAccess())
            {
                Entity closestTarget = Entity.Null;
                float minDistance = attackData.ValueRO.DetectionRange;
                float3 playerPos = transform.ValueRO.Position;
                float2 movement = movementData.ValueRO.MoveDirection;
                float distance = 0f;
                
                // 움직임이 없다면 Idle로 변경
                if (math.lengthsq(movement) <= float.Epsilon)
                {
                    attackData.ValueRW.Reset();
                    ecb.SetComponentEnabled<MoveState>(entity, false);
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}