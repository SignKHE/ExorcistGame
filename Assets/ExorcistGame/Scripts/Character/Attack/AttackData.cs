using System;
using Unity.Entities;

namespace ExorcistGame.Character
{
    [Serializable]
    public struct AttackData : IComponentData
    {
        public Entity Target;
        [Label("공격 범위")]
        public float AttackRange;
        [Label("공격 시간")]
        public float AttackTime;
        [NonSerialized]
        public float AttackTimer;
        [Label("재장전 시간")]
        public float ReloadTime;
        [NonSerialized]
        public float ReloadTimer;

        public AttackData Reset()
        {
            Target = Entity.Null;
            AttackTimer = 0f;
            ReloadTimer = 0f;
            return this;
        }
    }
}