using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Level
{
    [UpdateBefore(typeof(LevelSystem))]
    public partial struct ExpSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // 레벨 설정 싱글톤 가져오기. (없으면 종료)
            if(!SystemAPI.TryGetSingleton<LevelConfig>(out var config)) return;
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (data, entity) 
                     in SystemAPI.Query<RefRW<LevelData>>()
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
                
                uint level = exp / config.MaxExperience;
                exp %= config.MaxExperience;
                data.ValueRW.Experience = exp;

                if (level > 0)
                {
                    ecb.AppendToBuffer(entity, new LevelGainBuffer() {Gain = level});
                }
            }
            
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}