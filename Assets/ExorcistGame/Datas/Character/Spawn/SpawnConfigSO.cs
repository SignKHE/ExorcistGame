using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "SpawnConfigData", menuName = "ExorcistGame/SpawnConfig")]
    public class SpawnConfigSO : ScriptableObject
    {
        [Label("스폰 반경")]
        public float Radius;
        [Label("최대 스폰 갯수")]
        public int SpawnMax;
        [Label("스폰할 몬스터 프리팹")]
        public GameObject monsterPrefab;
    }
}
