using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Damage
{
    [BurstCompile]
    public partial struct InitializeHPSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (hpData,entity) 
                     in SystemAPI.Query<RefRW<HPData>>().WithEntityAccess())
            {
                if (hpData.ValueRO.IsInitialized) continue;

                ecb.AddBuffer<DamageBufferElement>(entity);
                ecb.AddComponent(entity, new DeadState());
                ecb.SetComponentEnabled<DeadState>(entity, false);
                hpData.ValueRW.IsInitialized = true;
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}