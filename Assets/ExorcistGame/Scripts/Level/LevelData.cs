using Unity.Entities;

namespace ExorcistGame.Level
{
    public struct LevelData : IComponentData
    {
        public uint Level;
        public uint Experience;
    }
}