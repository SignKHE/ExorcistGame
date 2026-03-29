using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill.BasicAttack
{
    /// <summary>
    /// 기본공격 엔티티를 위한 Authoring
    /// </summary>
    public class BasicAttackAuthoring : MonoBehaviour
    {
        [Label("투사체 스포너 데이터")]
        public ProjectileSpawnerSO ProjectileSpawnerData;
        public class BasicAttackBaker : Baker<BasicAttackAuthoring>
        {
            public override void Bake(BasicAttackAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new BasicAttackTag());
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