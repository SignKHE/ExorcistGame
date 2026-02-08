using ExorcistGame.Spawn;
using UnityEngine;

namespace ExorcistGame
{
    public class GameSceneMng : MonoBehaviour
    {
        [SerializeField]
        private SpawnManager spawnManager;

        private float _leftTime = 600f;
        
        private async void Start()
        {
            Debug.Log("GameSceneMng Start");

            await Awaitable.WaitForSecondsAsync(3f);
            
            GameLogic();
            
        }

        private async void GameLogic()
        {
            Debug.Log($"게임로직 시작");
            
            while (_leftTime >= 0f)
            {
                Spawn();
                await Awaitable.WaitForSecondsAsync(1f);
                Debug.Log($"카운트 다운");
                _leftTime -= 1f;
            }
        }

        private void Spawn()
        {
            Debug.Log($"스폰 명령");
            int spawnCount = 0;

            if (_leftTime > 480)
            {
                spawnCount = 4;
            }
            else if (_leftTime > 300)
            {
                spawnCount = 5;
            }
            else
            {
                spawnCount = 6;
            }
            
            spawnManager.RequestSpawn( spawnCount , 5f);
        }
        
        
    }
}
