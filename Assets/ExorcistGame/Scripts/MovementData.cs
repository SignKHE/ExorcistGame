using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame
{
    public struct MovementData : IComponentData
    {
        public float3 Direction;
        public float Speed;
    }
}

