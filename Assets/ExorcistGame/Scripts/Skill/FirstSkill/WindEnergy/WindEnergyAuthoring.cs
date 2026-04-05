using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill.FirstSkill
{
    public class WindEnergyAuthoring : MonoBehaviour
    {
        [Label("투사체 스포너 데이터")]
        public ProjectileSpawnerSO ProjectileSpawnerData;
        public class WindEnergyBaker : Baker<WindEnergyAuthoring>
        {
            public override void Bake(WindEnergyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new WindEnergyTag());
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