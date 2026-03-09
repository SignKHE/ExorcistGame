using UnityEngine.Events;

namespace ExorcistGame.Level
{
    public interface ILevelUpEvent
    {
        public UnityEvent LevelUpEvent { get; }
    }
}