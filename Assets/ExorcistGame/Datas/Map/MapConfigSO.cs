using ExorcistGame.Map;
using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "MapConfigData", menuName = "ExorcistGame/MapConfig")]
    public class MapConfigSO : ScriptableObject
    {
        [Label("사용할 청크의 사이즈")]
        public float ChunkSize;
        [Label("시야 거리 (청크 단위)")]
        public int ViewDistance;
        [Label("청크 프리팹")]
        public GameObject ChunkPrefab;
    }
}
