using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    public struct ProjectileData : IComponentData
    {
        /// <summary>
        /// 투사체 데미지
        /// </summary>
        public float Damage;
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
        /// <summary>
        /// 투사체 생성 주체
        /// </summary>
        public Entity Instigator;

        ProjectileData(float damage, float3 direction, float speed, float lifeTime, Entity instigator, EProjectileTriggerType triggerType = EProjectileTriggerType.SingleTarget)
        {
            Damage = damage;
            Direction = direction;
            Speed = speed;
            LifeTime = lifeTime;
            LeftLife = lifeTime;
            TriggerType = triggerType;
            Instigator = instigator;
        }
    }

    public enum EProjectileTriggerType : byte
    {
        SingleTarget,
        MultiTarget
    }
}