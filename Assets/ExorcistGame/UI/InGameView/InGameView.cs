using R3;
using TMPro;
using UnityEngine;

namespace ExorcistGame.UI.InGameView
{
    public class InGameView : ViewBase
    {
        [SerializeField]
        private TextMeshProUGUI timerText;
        protected override void Initialize()
        {
            (viewModel as InGameViewModel)?.LeftTime
                .Select(Mathf.CeilToInt) // float값 int로 변환
                .DistinctUntilChanged() // 변환한 값이 변했을때만 반응
                .Subscribe(leftTime =>
                {
                    int minutes = leftTime / 60;
                    int seconds = leftTime % 60;
                    timerText.text = $"{minutes:D2}:{seconds:D2}";
                })
                .AddTo(disposables);
        }

        protected override void Reset()
        {
            timerText.text = "00:00";
        }
    }
}