using ExorcistGame.Damage;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace ExorcistGame.Skill
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ProjectileHitSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            
        }
    }
    
    [BurstCompile]
    public struct ProjectileTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<ProjectileData> ProjectileLookup;
        [ReadOnly] public ComponentLookup<HPData> TargetLookup;
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isAProjectile = ProjectileLookup.HasComponent(entityA);
            bool isBTarget = TargetLookup.HasComponent(entityB);

            bool isBProjectile = ProjectileLookup.HasComponent(entityB);
            bool isATarget = TargetLookup.HasComponent(entityA);
            
            // A와 B가 각각 투사체와 타겟일때 충돌 판정
            if ((isAProjectile && isBTarget) || (isBProjectile && isATarget))
            {
                // 투사체 엔티티와 타겟 엔티티를 알기 쉽게 할당
                var (projectileEntity, targetEntity) = isAProjectile ? (entityA, entityB) : (entityB, entityA);
                
                bool projectileIsPlayer = PlayerLookup.HasComponent(projectileEntity);
                bool targetIsPlayer = PlayerLookup.HasComponent(targetEntity);

                // 투사체와 타겟이 같은 편이 아닐때 충돌 판정
                if (projectileIsPlayer != targetIsPlayer)
                {
                    // 타겟에게 데미지
                }
            }
        }
    }
}