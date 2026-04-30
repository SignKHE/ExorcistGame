

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExorcistGame.UI.LevelUpView
{
    public class LevelUpView : ViewBase
    {
        [SerializeField] private List<EnergyButtonView> energyButtons;
        [SerializeField] private CanvasGroup energyButtonsGroup;
        [SerializeField] private GameObject dragPanel;
        [SerializeField] private Button completeButton;

        public int SkillCnt { get; private set; }

        private int _energyCnt = 0;
        
        protected override void Initialize()
        {
            SkillCnt = 0;
            foreach (EnergyButtonView energyButton in energyButtons)
            {
                energyButton.AddBeginDragEvent(EnergyButtonBeginDrag);
                energyButton.AddBeginDragEvent(EnergyButtonEndDrag);
            }
            completeButton.onClick.AddListener(Complete);
        }

        protected void Complete()
        {
            (viewModel as LevelUpViewModel)?.Complete();
            Destroy(gameObject);
        }

        protected override void Reset()
        {
            
        }

        private void EnergyButtonBeginDrag()
        {
            _energyCnt = SkillCnt;
            energyButtonsGroup.alpha = 0f;
            energyButtonsGroup.enabled = false;
        }

        private void EnergyButtonEndDrag()
        {
            if (_energyCnt == SkillCnt)
            {
                EnergyButtonCancel();
            }
        }

        private void EnergyButtonCancel()
        {
            energyButtonsGroup.alpha = 1f;
            energyButtonsGroup.enabled = true;
        }

        private void OnDestroy()
        {
            viewModel.Dispose();
        }
    }
}