using System;
using UnityEngine;

namespace ExorcistGame.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup worldCanvasGroup;
        [SerializeField]
        private CanvasGroup mainCanvasGroup;
        [SerializeField]
        private CanvasGroup popupCanvasGroup;

        public static UIManager Instance { get; private set; }
        
        /// <summary>
        /// UI 타입
        /// </summary>
        public enum EUIType
        {
            /// <summary>
            /// 월드 타입. (HP바, 네임태그)
            /// </summary>
            World,
            /// <summary>
            /// 메인 타입. (인게임 UI)
            /// </summary>
            Main,
            /// <summary>
            /// 팝업 타입. (선택창)
            /// </summary>
            Popup
        }

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// UI 생성
        /// </summary>
        /// <param name="viewPrefab">UI 프리팹</param>
        /// <param name="viewModel">View가 바인딩할 View Model</param>
        /// <param name="uiType">UI 타입</param>
        /// <returns></returns>
        public void CreateUI(GameObject viewPrefab, ViewModelBase viewModel, EUIType uiType = EUIType.Main)
        {
            Debug.Log("Create UI");
            
            // View 프리팹 생성
            ViewBase view = Instantiate(original: viewPrefab,
                parent: uiType switch
                {
                    EUIType.World => worldCanvasGroup.transform,
                    EUIType.Main => mainCanvasGroup.transform,
                    EUIType.Popup => popupCanvasGroup.transform
                }).GetComponent<ViewBase>();
            
            // View 초기화
            view.Initialize(viewModel);
        }
    }
}
