using Unity.Entities;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 스킬 생성 요청
    /// </summary>
    public struct SkillCreateRequest : IBufferElementData
    {
        public ESkill RequestSkill;
    }
}