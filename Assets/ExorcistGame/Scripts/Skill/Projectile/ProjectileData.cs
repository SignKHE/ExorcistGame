using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 투사체 정보
    /// </summary>
    [Serializable]
    public struct ProjectileData : IComponentData, IEnableableComponent
    {
        /// <summary>
        /// 투사체 데미지
        /// </summary>
        [Label("데미지")]
        public float Damage;
        /// <summary>
        /// 수명
        /// </summary>
        [Label("수명")]
        public float LifeTime;
        /// <summary>
        /// 남은 수명
        /// </summary>
        [NonSerialized]
        public float LeftLife;
        /// <summary>
        /// 투사체 충돌 유형
        /// </summary>
        [Label("투사체 유형")]
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

        public ProjectileData SetInstigator(Entity instigator)
        {
            Instigator = instigator;
            return this;
        }

        public ProjectileData Reset()
        {
            LeftLife = LifeTime;
            return this;
        }
    }

    public enum EProjectileTriggerType : byte
    {
        [InspectorName("단일 대상")]
        SingleTarget,
        [InspectorName("다수 대상")]
        MultiTarget
    }
}