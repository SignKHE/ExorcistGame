using R3;
using UnityEngine;

namespace ExorcistGame.UI
{
    /// <summary>
    /// UI View
    /// ViewModel을 통해 상태를 업데이트
    /// 상호작용 이벤트를 ViewModel을 통해 처리
    /// </summary>
    public abstract class ViewBase : MonoBehaviour
    {
        /// <summary>
        /// View가 바인딩하는 ViewModel
        /// </summary>
        protected ViewModelBase viewModel;
        /// <summary>
        /// View의 캔버스 그룹
        /// </summary>
        [SerializeField]
        protected CanvasGroup viewCanvasGroup;
        
        /// <summary>
        /// Dispose할 바인딩 모음
        /// </summary>
        private readonly CompositeDisposable _disposables = new();

        /// <summary>
        /// View 초기화
        /// </summary>
        /// <param name="viewModel">View가 구독할 ViewModel</param>
        public void Initialize(ViewModelBase newViewModel)
        {
            Debug.Log("View Initialize");
            // 전달된 ViewModel이 Null이라면 경고 후 종료.
            if (newViewModel == null)
            {
                Debug.LogError("ViewModel is null");
                return;
            }
            
            // 기존 ViewModel과의 관계를 정리합니다.
            _disposables.Clear();
            
            // UI 상태 리셋.
            Reset();
            
            // 새로운 ViewModel으로 변경
            viewModel = newViewModel;
            
            // 초기화 실행
            Initialize();
        }
        
        /// <summary>
        /// 전달 받은 ViewModel의 바인딩을 수행하는 역할.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// UI를 처음 상태로 되돌리기 위한 역할.
        /// </summary>
        protected abstract void Reset();
    }
}