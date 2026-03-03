using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [BurstCompile]
    public partial struct ProjectileMovementSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (data, transform, entity) 
                     in SystemAPI.Query<RefRO<ProjectileData>, RefRW<LocalTransform>>().WithNone<Disabled>().WithEntityAccess() )
            {
                float3 velocity = data.ValueRO.Direction * data.ValueRO.Speed * deltaTime;
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(data.ValueRO.Direction, math.up());
                transform.ValueRW.Position += velocity;
            }
        }
    }
}