using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Skill
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [BurstCompile]
    public partial struct ProjectileLifeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            float deltaTime = SystemAPI.Time.DeltaTime;

            // 2. 메인 스레드에서 쿼리를 순회합니다. (엔티티 ID가 필요하므로 WithEntityAccess 사용)
            foreach (var (data, entity) in SystemAPI.Query<RefRW<ProjectileData>>().WithEntityAccess())
            {
                data.ValueRW.LeftLife -= deltaTime;
                if (data.ValueRO.LeftLife <= 0)
                {
                    ecb.DestroyEntity(entity);
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}