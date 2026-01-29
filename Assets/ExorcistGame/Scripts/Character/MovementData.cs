using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Character
{
    public struct MovementData : IComponentData
    {
        public float2 MoveDirection;
    }
}

