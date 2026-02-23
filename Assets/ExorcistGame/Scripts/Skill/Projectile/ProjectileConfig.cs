using Unity.Entities;

namespace ExorcistGame.Skill
{
    public struct ProjectileConfig : IComponentData
    {
        /// <summary>
        /// 투사체 프리팹
        /// </summary>
        public Entity ProjectilePrefab;
    }
}