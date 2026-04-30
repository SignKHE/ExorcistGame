using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "ExorcistGame/SkillData")]
    public class SkillData : ScriptableObject
    {
        [Label("스킬 이미지")]
        public Sprite mainSprite;
        [Label("스킬 이름")]
        public string name;
        [Label("스킬 설명", 4, 8)]
        public string content;
    }
}
