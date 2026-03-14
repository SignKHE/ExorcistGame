using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Seeker
{
    public struct SeekerData :  IComponentData, IEnableableComponent
    {
        public float Range;
        public ETargetType TargetType;
    }

    public enum ETargetType : byte
    {
        Player,
        Monster
    }
}