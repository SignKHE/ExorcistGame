using ExorcistGame.Character.Spawn;
using ExorcistGame.Level;
using ExorcistGame.UI;
using ExorcistGame.UI.InGameView;
using ExorcistGame.UI.LevelUpView;
using R3;
using Unity.Entities;
using UnityEngine;

namespace ExorcistGame
{
    public class GameSceneMng : MonoBehaviour
    {
        [SerializeField]
        private SpawnManager spawnManager;

        [SerializeField] private LevelMng levelManager;
        [SerializeField]
        private GameObject inGameViewPrefab;
        [SerializeField]
        private GameObject levelUpViewPrefab;
        
        private GameTimer _gameTimer = new GameTimer(600.0f);
        private LevelModelSystem _levelModelSystem;

        private InGameViewModel _viewModel;
        
        private async void Start()
        {
            Debug.Log("GameSceneMng Start");
            
            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null) return;
            _levelModelSystem = world.GetExistingSystemManaged<LevelModelSystem>();

            _viewModel = new InGameViewModel(_gameTimer, _levelModelSystem);
            _viewModel.LevelValue.DistinctUntilChanged()
                .Where(x => x > 0)
                .Subscribe(level =>
                {
                    LevelUpEvent();
                }
            );
            CreateInGameUI();

            await Awaitable.WaitForSecondsAsync(3f);
            
            GameLogic();
            
        }

        private void CreateInGameUI()
        {
            UIManager.Instance.CreateUI(inGameViewPrefab,_viewModel, UIManager.EUIType.Main);
        }

        private void LevelUpEvent()
        {
            GamePause();
            Debug.Log("GameSceneMng LevelUpEvent");
            LevelUpViewModel levelUpViewModel = new LevelUpViewModel(GameResume);
            UIManager.Instance.CreateUI(levelUpViewPrefab,levelUpViewModel, UIManager.EUIType.Popup);
        }

        private void GamePause()
        {
            var simulationGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationGroup.Enabled = false;
            Time.timeScale = 0f;
        }

        private void GameResume()
        {
            var simulationGroup = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>();
            simulationGroup.Enabled = true;
            Time.timeScale = 1f;
        }
        
        private async void GameLogic()
        {
            Debug.Log($"게임로직 시작");
            
            while (_gameTimer.LeftTime.Value >= 0f)
            {
                if (Mathf.FloorToInt(_gameTimer.LeftTime.Value) % 2 == 0)
                {
                    Spawn();
                }
                await Awaitable.WaitForSecondsAsync(1f);
                Debug.Log($"카운트 다운");
                _gameTimer.CountDown();
            }
        }

        private void Spawn()
        {
            Debug.Log($"스폰 명령");
            int spawnCount = 0;

            if (_gameTimer.LeftTime.Value > 480)
            {
                spawnCount = 4;
            }
            else if (_gameTimer.LeftTime.Value > 300)
            {
                spawnCount = 5;
            }
            else
            {
                spawnCount = 6;
            }
            
            spawnManager.RequestSpawn( spawnCount , 5f);
        }

        public class GameTimer : ITimer
        {
            public GameTimer(float leftTime)
            {
                LeftTime = new ReactiveProperty<float>();
                LeftTime.Value = leftTime;
            }

            public void CountDown()
            {
                LeftTime.Value -= 1.0f;
            }

            public ReactiveProperty<float> LeftTime { get; }
        }
    }
}
