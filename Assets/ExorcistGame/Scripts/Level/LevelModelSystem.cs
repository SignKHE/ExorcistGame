using R3;
using Unity.Entities;

namespace ExorcistGame.Level
{
    public partial class LevelModelSystem : SystemBase, ILevelModel
    {
        protected override void OnUpdate()
        {
            foreach (var data in SystemAPI.Query<RefRO<LevelData>>().WithAll<PlayerTag>())
            {
                if (data.ValueRO.Level != Level.Value)
                {
                    Level.Value = data.ValueRO.Level;
                }

                if (data.ValueRO.Experience != Experience.Value)
                {
                    Experience.Value = data.ValueRO.Experience;
                }
            }
        }

        public ReactiveProperty<uint> Level { get; private set; }
        public ReactiveProperty<uint> Experience { get; private set; }
    }
}