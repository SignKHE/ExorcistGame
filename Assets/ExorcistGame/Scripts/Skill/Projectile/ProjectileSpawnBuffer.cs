using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    public struct ProjectileSpawnBuffer : IBufferElementData
    {
        public float3 SpawnLocation;
        public ProjectileData Data;
    }
}