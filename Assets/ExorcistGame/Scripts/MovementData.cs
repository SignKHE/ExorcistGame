using System;
using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame
{
    [Serializable]
    public struct MovementData : IComponentData
    {
        [NonSerialized]
        public float3 Direction;
        [Label("속도")]
        public float Speed;

        public MovementData Reset()
        {
            Direction = float3.zero;
            return this;
        }
    }
}

