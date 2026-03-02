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
        public bool IsInitialized;

        public HPData(float hp)
        {
            MaxHP = hp;
            HP = MaxHP;
            IsInitialized = false;
        }
    }
}