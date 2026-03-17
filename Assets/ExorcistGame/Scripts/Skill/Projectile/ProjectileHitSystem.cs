using ExorcistGame.Damage;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 투사체 충돌 처리 시스템
    /// </summary>
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    public partial struct ProjectileHitSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            
            // 싱글톤에서 ParallelWriter ECB 가져오기
            var ecbSingleton = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

            var job = new ProjectileTriggerJob
            {
                ProjectileLookup = SystemAPI.GetComponentLookup<ProjectileData>(true),
                TargetLookup = SystemAPI.GetComponentLookup<HPData>(true),
                PlayerLookup = SystemAPI.GetComponentLookup<PlayerTag>(true),
                ECB = ecb
            };
            
            // .Complete() 없이 Dependency만 연결해 메인 스레드 멈춤 방지
            state.Dependency = job.Schedule(simulation, state.Dependency);
        }
    }

    /// <summary>
    /// 투사체 충돌 처리 및 풀 반환을 한 번에 수행하는 Job
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

            if ((isAProjectile && isBTarget) || (isBProjectile && isATarget))
            {
                var (projectileEntity, targetEntity) = isAProjectile ? (entityA, entityB) : (entityB, entityA);
                
                bool projectileIsPlayer = PlayerLookup.HasComponent(projectileEntity);
                bool targetIsPlayer = PlayerLookup.HasComponent(targetEntity);

                // 아군 체크
                if (projectileIsPlayer == targetIsPlayer) return;
                
                var projData = ProjectileLookup[projectileEntity];
                int sortKey = projectileEntity.Index;

                ECB.AppendToBuffer(sortKey, targetEntity, new DamageBufferElement
                {
                    Value = projData.Damage, 
                    Instigator = projData.Instigator
                });

                // 투사체 비활성화
                ECB.AddComponent(sortKey, projectileEntity, LocalTransform.FromPosition(new float3(0, -100f, 0)));
                ECB.SetComponentEnabled<ProjectileData>(sortKey, projectileEntity, false);
                ECB.AddComponent(sortKey, projectileEntity, new Disabled());

                // 투사체 반납
                ECB.AppendToBuffer(sortKey, projData.Instigator, new ProjectileSpawnPoolBuffer 
                { 
                    ProjectileEntity = projectileEntity 
                });
            }
        }
    }
}