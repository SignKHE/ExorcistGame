using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    public struct ProjectileData : IComponentData
    {
        /// <summary>
        /// 방향
        /// </summary>
        public float3 Direction;
        /// <summary>
        /// 속도
        /// </summary>
        public float Speed;
        /// <summary>
        /// 수명
        /// </summary>
        public float LifeTime;
        /// <summary>
        /// 남은 수명
        /// </summary>
        public float LeftLife;
        /// <summary>
        /// 투사체 충돌 유형
        /// </summary>
        public EProjectileTriggerType TriggerType;
        
    }

    public enum EProjectileTriggerType : byte
    {
        Single,
        Multi
    }
}