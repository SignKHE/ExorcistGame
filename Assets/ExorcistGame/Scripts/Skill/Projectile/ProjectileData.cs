using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    public struct ProjectileData : IComponentData, IEnableableComponent
    {
        /// <summary>
        /// 투사체 데미지
        /// </summary>
        public float Damage;
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
        public ProjectileData(Entity instigator, float damage = 10f, float lifeTime = 1f, EProjectileTriggerType triggerType = EProjectileTriggerType.SingleTarget)
        {
            Damage = damage;
            LifeTime = lifeTime;
            LeftLife = lifeTime;
            TriggerType = triggerType;
            Instigator = instigator;
        }

        public void SetInstigator(Entity instigator)
        {
            Instigator = instigator;
        }
    }

    public enum EProjectileTriggerType : byte
    {
        SingleTarget,
        MultiTarget
    }
}