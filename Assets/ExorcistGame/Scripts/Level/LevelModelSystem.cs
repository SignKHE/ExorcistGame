using R3;
using Unity.Entities;

namespace ExorcistGame.Level
{
    public partial class LevelModelSystem : SystemBase, ILevelModel
    {
        protected override void OnCreate()
        {
            Level = new ReactiveProperty<uint>(0);
            Experience = new ReactiveProperty<uint>(0);
            MaxLevel =  new ReactiveProperty<uint>(0);
            MaxExperience = new ReactiveProperty<uint>(0);
        }

        protected override void OnUpdate()
        {
            foreach (var (data, config) in SystemAPI.Query<RefRO<LevelData>, RefRO<LevelConfig>>().WithAll<PlayerTag>())
            {
                if (data.ValueRO.Level != Level.Value)
                {
                    Level.Value = data.ValueRO.Level;
                    UnityEngine.Debug.Log(data.ValueRO.Level);
                }

                if (data.ValueRO.Experience != Experience.Value)
                {
                    Experience.Value = data.ValueRO.Experience;
                    UnityEngine.Debug.Log(data.ValueRO.Experience);
                }

                if (config.ValueRO.MaxLevel != MaxLevel.Value)
                {
                    MaxLevel.Value = config.ValueRO.MaxLevel;
                    UnityEngine.Debug.Log(config.ValueRO.MaxLevel);
                }

                if (config.ValueRO.MaxExperience != MaxExperience.Value)
                {
                    MaxExperience.Value = config.ValueRO.MaxExperience;
                    UnityEngine.Debug.Log(config.ValueRO.MaxExperience);
                }
            }
        }

        public ReactiveProperty<uint> Level { get; private set; }
        public ReactiveProperty<uint> Experience { get; private set; }
        public ReactiveProperty<uint> MaxLevel { get; private set; }
        public ReactiveProperty<uint> MaxExperience { get; private set; }
    }
}