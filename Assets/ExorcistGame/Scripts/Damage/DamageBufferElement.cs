using Unity.Entities;

namespace ExorcistGame.Damage
{
    public struct DamageBufferElement : IBufferElementData
    {
        /// <summary>
        /// 데미지 수치
        /// </summary>
        public float Value;
        /// <summary>
        /// 데미지 주체
        /// </summary>
        public Entity Instigator;
    }
}