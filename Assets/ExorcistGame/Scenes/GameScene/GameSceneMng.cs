using ExorcistGame.Spawn;
using ExorcistGame.UI;
using ExorcistGame.UI.InGameView;
using R3;
using UnityEngine;

namespace ExorcistGame
{
    public class GameSceneMng : MonoBehaviour
    {
        [SerializeField]
        private SpawnManager spawnManager;
        [SerializeField]
        private GameObject inGameViewPrefab;

        private GameTimer _gameTimer = new GameTimer(600.0f);

        private InGameViewModel _viewModel;
        
        private async void Start()
        {
            Debug.Log("GameSceneMng Start");

            CreateInGameUI();

            await Awaitable.WaitForSecondsAsync(3f);
            
            GameLogic();
            
        }

        private void CreateInGameUI()
        {
            _viewModel = new InGameViewModel(_gameTimer);
            UIManager.Instance.CreateUI(inGameViewPrefab,_viewModel, UIManager.EUIType.Main);
        }

        private async void GameLogic()
        {
            Debug.Log($"게임로직 시작");
            
            while (_gameTimer.LeftTime.Value >= 0f)
            {
                Spawn();
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
