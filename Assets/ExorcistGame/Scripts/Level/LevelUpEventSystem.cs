using ExorcistGame.Skill;
using R3;
using Unity.Entities;
using UnityEngine.Events;

namespace ExorcistGame.Level
{
    public partial class LevelUpEventSystem : SystemBase, ISkillPointModel, ILevelUpEvent
    {
        protected override void OnCreate()
        {
            LevelUpEvent = new UnityEvent();
        }

        protected override void OnUpdate()
        {
            if(!SystemAPI.TryGetSingletonBuffer<LevelUpEventBuffer>(out var levelUpEventBuffer) || levelUpEventBuffer.IsEmpty) return;
            foreach (var skillPoint in levelUpEventBuffer)
            {
                SkillPoint.Value += skillPoint.SkillPoint;
            }
            LevelUpEvent.Invoke();
        }

        public ReactiveProperty<uint> SkillPoint { get; private set; }
        public UnityEvent LevelUpEvent { get; private set; }
    }
}