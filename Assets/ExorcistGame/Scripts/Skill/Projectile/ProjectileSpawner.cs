using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Skill
{
    public struct ProjectileSpawner : IComponentData
    {
        /// <summary>
        /// 투사체 프리팹
        /// </summary>
        public Entity ProjectilePrefab;
        /// <summary>
        /// 스포너 초기화 상태값
        /// </summary>
        public bool IsInitialized;

        public int PoolSize;

        public ProjectileSpawner(Entity projectilePrefab)
        {
            ProjectilePrefab = projectilePrefab;
            IsInitialized = false;
            PoolSize = 10;
        }
    }
}