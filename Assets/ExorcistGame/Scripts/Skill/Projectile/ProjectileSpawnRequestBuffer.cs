using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 투사체 스포너의 투사체 생성 요청 버퍼
    /// </summary>
    public struct ProjectileSpawnRequestBuffer : IBufferElementData
    {
        public float3 SpawnLocation;
        public float3 Direction;
    }
}