using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Level
{
    public partial struct LevelSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            // 레벨 설정 싱글톤 가져오기. (없으면 종료)
            if(!SystemAPI.TryGetSingleton<LevelConfig>(out var config)) return;
            
            foreach (var (data, entity) 
                     in SystemAPI.Query<RefRW<LevelData>>()
                         .WithAll<ExpGainBuffer, LevelGainBuffer>()
                         .WithEntityAccess())
            {
                var levelGainBuffer = SystemAPI.GetBuffer<LevelGainBuffer>(entity);
                if (levelGainBuffer.IsEmpty) continue;

                uint level = 0;
                foreach (var levelGain in levelGainBuffer)
                {
                    level += levelGain.Gain;
                }
                levelGainBuffer.Clear();
                
                // 얻는 레벨이 최대 레벨을 초과시킬 수 있다면 얻는 레벨량 조정
                level = config.MaxLevel > data.ValueRO.Level + level ? config.MaxLevel - data.ValueRO.Level : level;
                
                data.ValueRW.Level += level;
            }
        }
    }
}