using Unity.Entities;

namespace ExorcistGame.Character
{
    public struct AttackData : IComponentData
    {
        public Entity Target;
        public float DetectionRange;
        public float AttackRange;
        public float AttackTime;
        public float AttackTimer;
        public float ReloadTime;
        public float ReloadTimer;
    }
}