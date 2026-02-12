using Unity.Entities;

namespace ExorcistGame.Damage
{
    public struct HPData : IComponentData
    {
        /// <summary>
        /// 최대 체력
        /// </summary>
        public float MaxHP;
        /// <summary>
        /// 현재 체력
        /// </summary>
        public float HP;
    }
}