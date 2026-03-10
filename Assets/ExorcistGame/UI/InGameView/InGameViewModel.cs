using ExorcistGame.Level;
using R3;

namespace ExorcistGame.UI.InGameView
{
    public class InGameViewModel : ViewModelBase
    {
        private ITimer _timerModel;
        private ILevelModel _levelModel;
        
        public ReadOnlyReactiveProperty<float> LeftTime { get; }
        public ReadOnlyReactiveProperty<uint> LevelValue { get; }
        public ReadOnlyReactiveProperty<uint> ExperienceValue { get; }
        public ReadOnlyReactiveProperty<uint> MaxLevel { get; }
        public ReadOnlyReactiveProperty<uint> MaxExperience { get; }
        public InGameViewModel(ITimer timer, ILevelModel levelModel)
        {
            _timerModel = timer;
            LeftTime = _timerModel.LeftTime.ToReadOnlyReactiveProperty();
            
            _levelModel = levelModel;
            LevelValue = _levelModel.Level.ToReadOnlyReactiveProperty();
            ExperienceValue = _levelModel.Experience.ToReadOnlyReactiveProperty();
            MaxLevel = _levelModel.MaxLevel.ToReadOnlyReactiveProperty();
            MaxExperience = _levelModel.MaxExperience.ToReadOnlyReactiveProperty();
        }
        
        protected override void DisposeReactiveProperty()
        {
            LeftTime.Dispose();
            LevelValue.Dispose();
            ExperienceValue.Dispose();
            MaxLevel.Dispose();
            MaxExperience.Dispose();
        }
    }
}