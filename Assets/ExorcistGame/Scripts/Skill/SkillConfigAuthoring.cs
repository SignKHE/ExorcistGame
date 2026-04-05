using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 스킬 정보 Authoring
    /// </summary>
    public class SkillConfigAuthoring : MonoBehaviour
    {
        [SerializeField] private SkillConfigSO skillConfigData;
        
        public class LevelConfigBaker : Baker<SkillConfigAuthoring>
        {
            public override void Bake(SkillConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SkillConfig()
                {
                    BaseAttack = GetEntity(authoring.skillConfigData.basicAttack , TransformUsageFlags.Dynamic),
                    FireEnergy = GetEntity(authoring.skillConfigData.fireEnergy, TransformUsageFlags.Dynamic),
                    MetalEnergy = GetEntity(authoring.skillConfigData.metalEnergy, TransformUsageFlags.Dynamic),
                    WindEnergy = GetEntity(authoring.skillConfigData.windEnergy, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}