using Unity.Burst;
using Unity.Entities;

namespace ExorcistGame.Damage
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    [UpdateAfter(typeof(FixedStepSimulationSystemGroup))]
    public partial struct DamageSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (hpData, damageBuffer, entity) in SystemAPI.Query<RefRW<HPData>, DynamicBuffer<DamageBufferElement>>()
                         .WithNone<DeadState>()
                         .WithEntityAccess())
            {
                // 버퍼된 데미지 없으면 넘어가기
                if(damageBuffer.IsEmpty) continue;

                float totalDamage = 0f;

                // 버퍼에 쌓인 모든 데미지 누적
                foreach (var damageData in damageBuffer)
                {
                    totalDamage += damageData.Value;
                }
                
                // 누적 데미지 HP에 적용
                hpData.ValueRW.HP -=  totalDamage;
                
                // 사용 끝난 데미지 버퍼 정리
                damageBuffer.Clear();

                // 만약 HP가 0보다 같거나 작다면 죽음 처리
                if (hpData.ValueRO.HP <= 0)
                {
                    // HP가 0보다 작지 않게 처리
                    hpData.ValueRW.HP = 0;
                    // DeadState 컴포넌트를 가진 엔티티만 죽음 처리
                    if (SystemAPI.HasComponent<DeadState>(entity))
                    {
                        // DeadState를 활성화하여 죽음 상태로 만든다
                        SystemAPI.SetComponentEnabled<DeadState>(entity, true);
                    }
                }
            }
        }
    }
}