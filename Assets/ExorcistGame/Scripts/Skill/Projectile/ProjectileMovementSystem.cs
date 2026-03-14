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

            foreach (var (movementData, transform, entity) 
                     in SystemAPI.Query<RefRO<MovementData>, RefRW<LocalTransform>>()
                         .WithAll<ProjectileData>().WithEntityAccess() )
            {
                float3 velocity = movementData.ValueRO.Direction * movementData.ValueRO.Speed * deltaTime;
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(movementData.ValueRO.Direction, math.up());
                transform.ValueRW.Position += velocity;
            }
        }
    }
}