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

            foreach (var (transform, movementData) 
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MovementData>>()
                .WithAll<PlayerTag, CharacterTag>())
            {
                //movement 값이 유의미할 때만
                if (math.lengthsq(movementData.ValueRO.Direction) > float.Epsilon)
                {
                    transform.ValueRW.Position += movementData.ValueRO.Direction * movementData.ValueRO.Speed * deltaTime;
                    
                    transform.ValueRW.Rotation = quaternion.LookRotationSafe(movementData.ValueRO.Direction, math.up());
                }
            }
        }
    }
}