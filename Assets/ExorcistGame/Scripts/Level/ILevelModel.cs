using R3;

namespace ExorcistGame.Level
{
    public interface ILevelModel
    {
        ReactiveProperty<uint> Level { get; }
        ReactiveProperty<uint> Experience { get; }
    }
}