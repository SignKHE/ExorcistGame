using R3;

namespace ExorcistGame.UI.InGameView
{
    public class InGameViewModel : ViewModelBase
    {
        private ITimer _timerModel;
        
        public ReadOnlyReactiveProperty<float> LeftTime { get; }
        public InGameViewModel(ITimer timer)
        {
            _timerModel = timer;
            LeftTime = _timerModel.LeftTime
                .ToReadOnlyReactiveProperty();
        }
        
        protected override void DisposeReactiveProperty()
        {
            LeftTime.Dispose();
        }
    }
}