using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [BurstCompile]
    public partial struct ProjectileLifeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            // 2. 메인 스레드에서 쿼리를 순회합니다. (엔티티 ID가 필요하므로 WithEntityAccess 사용)
            foreach (var (data,transform, entity) 
                     in SystemAPI.Query<RefRW<ProjectileData>,RefRW<LocalTransform>>().WithEntityAccess())
            {
                data.ValueRW.LeftLife -= deltaTime;
                if (data.ValueRO.LeftLife <= 0)
                {
                    SystemAPI.SetComponentEnabled<ProjectileData>(entity,false);
                    transform.ValueRW = LocalTransform.FromPosition(0, -100, 0);
                    
                    var poolBuffer = SystemAPI.GetBuffer<ProjectileSpawnPoolBuffer>(data.ValueRO.Instigator);
                    poolBuffer.Add(new ProjectileSpawnPoolBuffer() { ProjectileEntity = entity});
                }
            }
        }
    }
}