using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "LevelConfigData", menuName = "ExorcistGame/LevelConfig")]
    public class LevelConfigSO : ScriptableObject
    {
        [Label("최대 레벨")]
        public uint MaxLevel;
        [Label("최대 경험치")]
        public uint MaxExperience;
    }
}
