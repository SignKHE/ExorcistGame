using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "SkillConfigData", menuName = "ExorcistGame/SkillConfig")]
    public class SkillConfigSO : ScriptableObject
    {
        [Label("기본 공격 엔티티")]
        public GameObject basicAttack;
        [Header("1차 스킬")]
        [Label("주작의 기운")]
        public GameObject fireEnergy;
        [Label("백호의 기운")]
        public GameObject metalEnergy;
        [Label("청룡의 기운")]
        public GameObject windEnergy;
    }
}