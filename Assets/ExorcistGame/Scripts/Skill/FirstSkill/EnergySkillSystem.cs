using ExorcistGame.Seeker;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill.FirstSkill
{
    [BurstCompile]
    public partial struct EnergySkillSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (skillData, requests, transform, parent, entity) 
                     in SystemAPI.Query<RefRW<EnergySkillData>, DynamicBuffer<SkillRequestBuffer>, RefRO<LocalToWorld>, RefRO<Parent>>()
                         .WithAny<FireEnergyTag,MetalEnergyTag,WindEnergyTag>()
                         .WithEntityAccess())
            {
                skillData.ValueRW.Timer += 1f * deltaTime;
                if (skillData.ValueRO.Timer >= skillData.ValueRO.Time)
                {
                    skillData.ValueRW.Timer = 0;
                    if (SystemAPI.HasBuffer<FoundEntityBuffer>(parent.ValueRO.Value))
                    {
                        var foundEntities = SystemAPI.GetBuffer<FoundEntityBuffer>(parent.ValueRO.Value);
                        if (!foundEntities.IsEmpty)
                        {
                            var foundEntity = foundEntities[0];

                            if (SystemAPI.HasComponent<LocalTransform>(foundEntity.Value))
                            {
                                float3 targetPosition = SystemAPI.GetComponentRO<LocalTransform>(foundEntity.Value).ValueRO.Position;

                                ecb.AppendToBuffer(entity, new SkillRequestBuffer()
                                {
                                    TargetPosition = targetPosition
                                });
                            }
                        }
                    }
                }

                if (!requests.IsEmpty)
                {
                    foreach (var request in requests)
                    {
                        float3 playerPos = transform.ValueRO.Position;
                        float3 targetPos = request.TargetPosition;
                        float3 targetDirection = math.normalizesafe(targetPos - playerPos);
                    
                        ecb.AppendToBuffer( entity, new ProjectileSpawnRequestBuffer()
                        { 
                            SpawnLocation = playerPos + new float3(0f, 1f, 0f),
                            Direction = targetDirection
                        });
                    }
                
                    requests.Clear();
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}