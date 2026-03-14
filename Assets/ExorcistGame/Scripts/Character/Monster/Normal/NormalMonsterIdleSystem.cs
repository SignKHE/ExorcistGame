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
    public partial struct NormalMonsterIdleSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (movementData , foundEntityBuffers , entity)
                     in SystemAPI.Query<RefRW<MovementData>,DynamicBuffer<FoundEntityBuffer>>()
                         .WithAll<MonsterData, IdleState>().WithEntityAccess())
            {
                // 탐색기가 타겟을 찾았다면 움직이기 시작
                if (!foundEntityBuffers.IsEmpty)
                {
                    ecb.SetComponentEnabled<IdleState>(entity, false);
                    ecb.SetComponentEnabled<MoveState>(entity, true);
                    continue;
                }
                
                movementData.ValueRW.Direction = float3.zero;
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}