using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill.FirstSkill
{
    public class MetalEnergyAuthoring : MonoBehaviour
    {
        [Label("투사체 스포너 데이터")]
        public ProjectileSpawnerSO ProjectileSpawnerData;
        public class MetalEnergyBaker : Baker<MetalEnergyAuthoring>
        {
            public override void Bake(MetalEnergyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new MetalEnergyTag());
                AddComponent(entity, new EnergySkillData()
                {
                    Time = 10f,
                    Timer = 0f
                });
                AddComponent(entity, new ProjectileSpawner()
                {
                    PoolSize = authoring.ProjectileSpawnerData.Data.PoolSize,
                    ProjectilePrefab = GetEntity(authoring.ProjectileSpawnerData.ProjectilePrefab, TransformUsageFlags.Dynamic),
                    ProjectileSpeed = authoring.ProjectileSpawnerData.Data.ProjectileSpeed,
                    ProjectileDataPreset = (authoring.ProjectileSpawnerData.Data.ProjectileDataPreset).SetInstigator(entity).Reset()
                });
            }
        }
    }
}