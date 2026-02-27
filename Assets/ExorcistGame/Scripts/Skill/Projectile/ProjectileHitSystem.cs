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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            
            var hitRecordECB = new EntityCommandBuffer(Allocator.TempJob);
            
            var projectileLookup = SystemAPI.GetComponentLookup<ProjectileData>(true);
            var targetLookup = SystemAPI.GetComponentLookup<HPData>(true);
            var playerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true);
            
            var job = new ProjectileTriggerJob
            {
                ProjectileLookup = projectileLookup,
                TargetLookup = targetLookup,
                PlayerLookup = playerLookup,
                ECB = hitRecordECB.AsParallelWriter()
            };
            
            // 투사체 충돌 처리 실행
            var hitJobHandle = job.Schedule(simulation, state.Dependency);
            hitJobHandle.Complete();
            // 투사체 충돌 처리가 완료된 다음 ECB 처리
            hitRecordECB.Playback(state.EntityManager);
            hitRecordECB.Dispose();

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var hitResolveEcb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            
            var hitResolveJob = new ProjectileHitJob
            {
                ECB = hitResolveEcb
            };
            
            // 충돌한 투사체의 데미지 처리 실행
            state.Dependency = hitResolveJob.ScheduleParallel(state.Dependency);
        }
    }
    
    /// <summary>
    /// 투사체 충돌처리 구현
    /// </summary>
    [BurstCompile]
    public struct ProjectileTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<ProjectileData> ProjectileLookup;
        [ReadOnly] public ComponentLookup<HPData> TargetLookup;
        [ReadOnly] public ComponentLookup<PlayerTag> PlayerLookup;
        public EntityCommandBuffer.ParallelWriter ECB;

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
                    // 투사체에 Hit버퍼 생성.
                    int sortKey = projectileEntity.Index;

                    ECB.AddBuffer<HitBuffer>(sortKey, projectileEntity);
                    ECB.AppendToBuffer(sortKey, projectileEntity, 
                        new HitBuffer()
                        {
                            Target = targetEntity,
                            DamageData = new DamageBufferElement
                            {
                                Value = ProjectileLookup[projectileEntity].Damage, 
                                Instigator = ProjectileLookup[projectileEntity].Instigator
                            }
                        });
                    
                }
            }
        }
    }
    
    /// <summary>
    /// Hit 버퍼를 가진 투사체 엔치치의 데미지 처리를 구현
    /// </summary>
    [BurstCompile]
    public partial struct ProjectileHitJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;

        public void Execute(Entity projectileEntity, [EntityIndexInQuery] int sortKey, ref DynamicBuffer<HitBuffer> hits)
        {
            if (hits.Length > 0)
            {
                Entity targetEntity = hits[0].Target;

                ECB.AddBuffer<DamageBufferElement>(sortKey, targetEntity);
                ECB.AppendToBuffer(sortKey, targetEntity, hits[0].DamageData);
                ECB.DestroyEntity(sortKey, projectileEntity);
            }
        }
    }
}