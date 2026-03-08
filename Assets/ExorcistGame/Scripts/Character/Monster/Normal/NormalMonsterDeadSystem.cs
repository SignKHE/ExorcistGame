using ExorcistGame.Character.Spawn;
using ExorcistGame.Character.State;
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

            foreach (var (hpData, transform,spawnedData , entity)
                     in SystemAPI.Query< RefRW<HPData>, RefRO<LocalTransform>, RefRO<SpawnedData>>()
                         .WithAll<MonsterData, DeadState>().WithEntityAccess())
            {
                hpData.ValueRW.Reset();
                ecb.SetComponentEnabled<IdleState>(entity, false);
                ecb.SetComponentEnabled<MoveState>(entity, false);
                ecb.SetComponentEnabled<AttackState>(entity, false);
                ecb.SetComponentEnabled<DeadState>(entity, false);
                ecb.AddComponent(entity, new Disabled());
                ecb.AppendToBuffer(spawnedData.ValueRO.SpawnPool, new SpawnPoolBuffer() {LoadedEntity = entity});
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}