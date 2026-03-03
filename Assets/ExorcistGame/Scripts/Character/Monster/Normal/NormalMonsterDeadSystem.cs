using ExorcistGame.Damage;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ExorcistGame.Character.Monster.Normal
{
    [UpdateBefore(typeof(NormalMonsterAttackSystem))]
    [UpdateBefore(typeof(NormalMonsterIdleSystem))]
    [UpdateBefore(typeof(NormalMonsterMoveSystem))]
    [BurstCompile]
    public partial struct NormalMonsterDeadSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (transform, entity)
                     in SystemAPI.Query< RefRO<LocalTransform>>()
                         .WithAll<MonsterData, DeadState>().WithEntityAccess())
            {
                
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}