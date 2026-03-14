using Unity.Entities;

namespace ExorcistGame.Character
{
    public struct AttackData : IComponentData
    {
        public Entity Target;
        public float AttackRange;
        public float AttackTime;
        public float AttackTimer;
        public float ReloadTime;
        public float ReloadTimer;

        public void Reset()
        {
            Target = Entity.Null;
            AttackTimer = 0f;
            ReloadTimer = 0f;
        }
    }
}