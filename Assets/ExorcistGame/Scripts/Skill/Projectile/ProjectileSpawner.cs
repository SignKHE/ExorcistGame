using System;
using Unity.Entities;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 투사체 스포너 정보
    /// </summary>
    [Serializable]
    public struct ProjectileSpawner : IComponentData
    {
        /// <summary>
        /// 투사체 프리팹
        /// </summary>
        public Entity ProjectilePrefab;

        public ProjectileData ProjectileDataPreset;
        
        public int PoolSize;
        
        public float ProjectileSpeed;

        public ProjectileSpawner(Entity projectilePrefab,ProjectileData projectileDataPreset, int poolSize, float projectileSpeed= 8f)
        {
            ProjectilePrefab = projectilePrefab;
            ProjectileDataPreset = projectileDataPreset;
            PoolSize = poolSize;
            ProjectileSpeed = projectileSpeed;
        }
    }
}