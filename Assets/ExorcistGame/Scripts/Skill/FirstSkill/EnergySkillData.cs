using Unity.Entities;

namespace ExorcistGame.Skill.FirstSkill
{
    public struct EnergySkillData : IComponentData
    {
        public float Time;
        public float Timer;
    }
}