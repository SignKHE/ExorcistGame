using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 스킬 엔티티 생성 시스템
    /// </summary>
    [BurstCompile]
    public partial struct SkillCreateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            if( !SystemAPI.TryGetSingleton<SkillConfig>(out var skillConfig) ) return;
            
            foreach (var (requests, entity) 
                     in SystemAPI.Query<DynamicBuffer<SkillCreateRequest>>()
                         .WithEntityAccess())
            {
                foreach (var request in requests)
                {
                    Entity requestEntity = request.RequestSkill switch
                    {
                        ESkill.BasicAttack => skillConfig.BaseAttack,
                        ESkill.FireEnergy => skillConfig.FireEnergy,
                        ESkill.MetalEnergy => skillConfig.MetalEnergy,
                        ESkill.WindEnergy => skillConfig.WindEnergy,
                        _ => Entity.Null
                    };
                    
                    if(requestEntity == Entity.Null) continue;

                    Entity createEntity = ecb.Instantiate(requestEntity);
                    ecb.AddComponent(createEntity, LocalTransform.Identity);
                    ecb.AddComponent(createEntity, new Parent()
                    {
                        Value = entity
                    });
                    ecb.AddBuffer<SkillRequestBuffer>(createEntity);
                    
                    if(SystemAPI.HasComponent<PlayerTag>(entity))
                        ecb.AddComponent(createEntity, new PlayerTag());
                }
                requests.Clear();
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}