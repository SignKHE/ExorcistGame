using ExorcistGame.Spawn;
using UnityEngine;

namespace ExorcistGame
{
    public class GameSceneMng : MonoBehaviour
    {
        [SerializeField]
        private SpawnManager spawnManager;

        private async void Start()
        {
            Debug.Log("GameSceneMng Start");

            await Awaitable.WaitForSecondsAsync(3f);
            spawnManager.RequestSpawn(Vector3.zero, 30, 10f);
            await Awaitable.WaitForSecondsAsync(3f);
            spawnManager.RequestSpawn(Vector3.zero, 30, 10f);
            await Awaitable.WaitForSecondsAsync(3f);
            spawnManager.RequestSpawn(Vector3.zero, 30, 10f);
        }
    }
}
