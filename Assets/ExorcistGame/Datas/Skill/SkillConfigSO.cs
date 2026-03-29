using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "SkillConfigData", menuName = "ExorcistGame/SkillConfig")]
    public class SkillConfigSO : ScriptableObject
    {
        [Label("기본 공격 엔티티")]
        public GameObject basicAttack;
    }
}