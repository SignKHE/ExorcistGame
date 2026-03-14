using Unity.Entities;

namespace ExorcistGame.Level
{
    public struct LevelUpEventBuffer : IBufferElementData
    {
        public uint SkillPointGain;
    }
}