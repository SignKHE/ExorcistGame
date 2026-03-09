using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Level
{
    public class LevelConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private uint maxLevel;
        [SerializeField]
        private uint maxExperience;
        public class LevelConfigBaker : Baker<LevelConfigAuthoring>
        {
            public override void Bake(LevelConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new LevelConfig()
                {
                    MaxLevel = authoring.maxLevel,
                    MaxExperience = authoring.maxExperience
                });
                AddComponent(entity, new LevelData() {Experience = 0, Level = 0});
                AddComponent(entity, new PlayerTag());
                AddBuffer<ExpGainBuffer>(entity);
                AddBuffer<LevelGainBuffer>(entity);
                AddBuffer<LevelUpEventBuffer>(entity);
            }
        }
    }
}