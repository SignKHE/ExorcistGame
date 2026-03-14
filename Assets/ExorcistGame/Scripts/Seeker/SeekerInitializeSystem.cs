using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Seeker
{
    [BurstCompile]
    public partial struct SeekerInitializeSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (seeker, entity) 
                     in SystemAPI.Query<RefRO<SeekerData>>().WithNone<FoundEntityBuffer>().WithEntityAccess())
            {
                ecb.AddBuffer<FoundEntityBuffer>(entity);
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}