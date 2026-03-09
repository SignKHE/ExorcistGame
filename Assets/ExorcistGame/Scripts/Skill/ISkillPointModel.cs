using R3;

namespace ExorcistGame.Skill
{
    public interface ISkillPointModel
    {
        public ReactiveProperty<uint> SkillPoint { get; }
    }
}