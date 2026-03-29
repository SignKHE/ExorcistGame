using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 스킬 요청 버퍼
    /// </summary>
    public struct SkillRequestBuffer : IBufferElementData
    {
        public float3 TargetPosition;
    }
}