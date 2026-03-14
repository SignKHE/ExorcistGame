using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    public struct ProjectileSpawnRequestBuffer : IBufferElementData
    {
        public float3 SpawnLocation;
        public float3 Direction;
    }
}