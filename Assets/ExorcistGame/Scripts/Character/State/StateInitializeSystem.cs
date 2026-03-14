using Unity.Collections;
using Unity.Entities;

namespace ExorcistGame.Character.State
{
    public partial struct StateInitializeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (stateData, entity) 
                     in SystemAPI.Query<RefRW<StateData>>().WithEntityAccess())
            {
                if (!stateData.ValueRO.IsInitialized)
                {
                    ecb.AddComponent(entity, new IdleState());
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                    ecb.AddComponent(entity, new MoveState());
                    ecb.SetComponentEnabled<MoveState>(entity, false);
                    ecb.AddComponent(entity, new AttackState());
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    stateData.ValueRW.IsInitialized = true;
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}