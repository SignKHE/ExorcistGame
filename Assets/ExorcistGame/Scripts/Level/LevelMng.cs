using System;
using ExorcistGame.Skill;
using ExorcistGame.UI.LevelUpView;
using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Level
{
    /// <summary>
    /// 레벨업 UI 생성등 레거시 방식에서의 기능을 위한 레벨 매니저
    /// </summary>
    public class LevelMng : MonoBehaviour
    {
        [SerializeField]
        private GameObject levelUpViewPrefab;

        private ISkillPointModel _skillPointModel;
        private ILevelUpEvent _levelUpEvent;

        private LevelUpViewModel _viewModel;

        private void Start()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null) return;
            _skillPointModel = world.GetExistingSystemManaged<LevelUpEventSystem>();
                
            _levelUpEvent.LevelUpEvent.AddListener(LevelUp);
        }

        private void LevelUp()
        {
            Debug.Log("LevelUp");
        }
    }
}