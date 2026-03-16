using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Level
{
    public class LevelConfigAuthoring : MonoBehaviour
    {
        [SerializeField] private LevelConfigSO data;
        public class LevelConfigBaker : Baker<LevelConfigAuthoring>
        {
            public override void Bake(LevelConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new LevelConfig()
                {
                    MaxLevel = authoring.data.MaxLevel,
                    MaxExperience = authoring.data.MaxExperience
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