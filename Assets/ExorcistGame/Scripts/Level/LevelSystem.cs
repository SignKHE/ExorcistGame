using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Level
{
    [BurstCompile]
    public partial struct LevelSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (data,config, entity) 
                     in SystemAPI.Query<RefRW<LevelData>, RefRO<LevelConfig>>()
                         .WithAll<LevelGainBuffer, LevelUpEventBuffer>()
                         .WithEntityAccess())
            {
                var levelGainBuffer = SystemAPI.GetBuffer<LevelGainBuffer>(entity);
                if (levelGainBuffer.IsEmpty) continue;

                // 현재 레벨이 최대 레벨보다 작을때만 레벨업 처리
                if (data.ValueRO.Level < config.ValueRO.MaxLevel)
                {
                    uint level = 0;
                    foreach (var levelGain in levelGainBuffer)
                    {
                        level += levelGain.Gain;
                    }

                    // 얻는 레벨이 최대 레벨을 초과시킬 수 있다면 얻는 레벨량 조정
                    level = config.ValueRO.MaxLevel > data.ValueRO.Level + level ? config.ValueRO.MaxLevel - data.ValueRO.Level : level;

                    data.ValueRW.Level += level;
                    ecb.AppendToBuffer(entity, new LevelUpEventBuffer() {SkillPoint = level});
                }
                levelGainBuffer.Clear();
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}