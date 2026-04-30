
using UnityEngine.Events;

namespace ExorcistGame.UI.LevelUpView
{
    public class LevelUpViewModel : ViewModelBase
    {
        private readonly UnityEvent _onComplete = new UnityEvent();
        
        public LevelUpViewModel(UnityAction completeAction)
        {
            _onComplete.AddListener(completeAction);
        }
        
        protected override void DisposeReactiveProperty()
        {
            
        }

        public void Complete()
        {
            _onComplete.Invoke();
        }
    }
}