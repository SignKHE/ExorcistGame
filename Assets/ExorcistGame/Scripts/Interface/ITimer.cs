using R3;

namespace ExorcistGame
{
    public interface ITimer
    {
        public ReactiveProperty<float> LeftTime { get; }
    }
}