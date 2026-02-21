using Unity.Entities;

namespace ExorcistGame.Character.State
{
    public struct StateData : IComponentData
    {
        public EState State;
    }
    
    [System.Flags]
    public enum EState : byte
    {
        Idle = 0,
        Move = 1,
        Attack = 2,
    }
}