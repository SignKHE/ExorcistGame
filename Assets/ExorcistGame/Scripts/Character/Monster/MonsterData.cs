using System;
using Unity.Entities;

namespace ExorcistGame.Character
{
    public struct MonsterData : IComponentData
    {
        public EMonsterType Type;
    }

    [Flags]
    public enum EMonsterType : byte
    {
        Normal = 1 << 0,
        Elite = 1 << 1,
        Boss = 1 << 2,
    }
}