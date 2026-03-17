using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 움직임이 가능한 투사체의 움직임 구현 시스템
    /// </summary>
    [BurstCompile]
    public partial struct ProjectileMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (movementData, transform, entity) 
                     in SystemAPI.Query<RefRO<MovementData>, RefRW<LocalTransform>>()
                         .WithAll<ProjectileData>().WithEntityAccess())
            {
                float3 velocity = movementData.ValueRO.Direction * movementData.ValueRO.Speed * deltaTime;
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(movementData.ValueRO.Direction, math.up());
                transform.ValueRW.Position += velocity;
            }
        }
    }
}