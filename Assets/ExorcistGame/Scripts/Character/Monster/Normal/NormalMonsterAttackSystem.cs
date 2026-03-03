using ExorcistGame.Character.State;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
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

            foreach (var (attackData, movementData, transform, entity)
                     in SystemAPI.Query<RefRW<AttackData>, RefRO<MovementData>, RefRO<LocalTransform>>()
                         .WithAll<MonsterData, AttackState>().WithEntityAccess())
            {
                
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}