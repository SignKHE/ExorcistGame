using System;
using Unity.Entities;

namespace ExorcistGame.Character.Monster
{
    public struct MonsterData : IComponentData
    {
        public EMonsterClassType ClassType;
        public EMonsterSpeciesType SpeciesType;
    }

    [Flags]
    public enum EMonsterClassType : byte
    {
        Normal = 1 << 0,
        Elite = 1 << 1,
        Boss = 1 << 2,
    }

    public enum EMonsterSpeciesType : byte
    {
        None = 0
    }
}