using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character
{
    [BurstCompile]
    public partial struct CharacterMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;
            float speed = 5f;

            foreach (var (transform, movementDirection) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementData>>()
                .WithAll<PlayerTag, CharacterTag>())
            {
                float2 movement = movementDirection.ValueRO.MoveDirection;

                //movement 값이 유의미할 때만
                if (math.lengthsq(movement) > float.Epsilon)
                {
                    float3 direction = new float3(movement.x, 0f, movement.y);
                    
                    transform.ValueRW.Position += direction * speed * deltaTime;
                    
                    transform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());
                }
            }
        }
    }
}