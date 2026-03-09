using Unity.Entities;

namespace ExorcistGame.Level
{
    public struct LevelConfig : IComponentData
    {
        public uint MaxLevel;
        public uint MaxExperience;
    }
}