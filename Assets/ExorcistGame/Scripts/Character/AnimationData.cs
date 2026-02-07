using Unity.Entities;

namespace ExorcistGame.Character
{
    public struct AnimationData : IComponentData
    {
        public float Speed;
        public bool AttackTrigger;
    }
}