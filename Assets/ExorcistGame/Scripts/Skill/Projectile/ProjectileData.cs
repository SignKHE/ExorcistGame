using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    public struct ProjectileData : IComponentData
    {
        public float3 Direction;
        public float Speed;
    }
}