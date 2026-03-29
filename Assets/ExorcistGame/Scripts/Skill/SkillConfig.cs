using Unity.Entities;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 스킬 정보 데이터
    /// </summary>
    public struct SkillConfig : IComponentData
    {
        public Entity BaseAttack;
    }
}