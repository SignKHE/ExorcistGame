using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Level
{
    [UpdateBefore(typeof(LevelSystem))]
    [BurstCompile]
    public partial struct ExpSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (data,config, entity) 
                     in SystemAPI.Query<RefRW<LevelData>,RefRO<LevelConfig>>()
                         .WithAll<ExpGainBuffer, LevelGainBuffer>()
                         .WithEntityAccess())
            {
                var expGainBuffer = SystemAPI.GetBuffer<ExpGainBuffer>(entity);
                if (expGainBuffer.IsEmpty) continue;

                uint exp = data.ValueRO.Experience;
                foreach (var expGain in expGainBuffer)
                {
                    exp += expGain.Gain;
                }
                expGainBuffer.Clear();
                
                uint maxExp = config.ValueRO.MaxExperience == 0 ? 1 : config.ValueRO.MaxExperience;
                uint level = exp / maxExp;
                exp %= maxExp;
                data.ValueRW.Experience = exp;

                if (level > 0)
                {
                    SystemAPI.GetBuffer<LevelGainBuffer>(entity).Add(new LevelGainBuffer() { Gain = level });
                }
            }
        }
    }
}