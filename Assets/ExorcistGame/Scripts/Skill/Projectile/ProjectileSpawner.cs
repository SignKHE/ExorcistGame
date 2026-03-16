using System;
using Unity.Entities;

namespace ExorcistGame.Skill
{
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
        /// <summary>
        /// 스포너 초기화 상태값
        /// </summary>
        public bool IsInitialized;

        public ProjectileSpawner(Entity projectilePrefab,ProjectileData projectileDataPreset, int poolSize, float projectileSpeed= 8f)
        {
            ProjectilePrefab = projectilePrefab;
            ProjectileDataPreset = projectileDataPreset;
            IsInitialized = false;
            PoolSize = poolSize;
            ProjectileSpeed = projectileSpeed;
        }
    }
}