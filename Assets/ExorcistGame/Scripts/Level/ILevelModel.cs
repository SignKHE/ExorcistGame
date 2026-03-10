using R3;

namespace ExorcistGame.Level
{
    public interface ILevelModel
    {
        ReactiveProperty<uint> Level { get; }
        ReactiveProperty<uint> Experience { get; }
        ReactiveProperty<uint> MaxLevel { get; }
        ReactiveProperty<uint> MaxExperience { get; }
    }
}