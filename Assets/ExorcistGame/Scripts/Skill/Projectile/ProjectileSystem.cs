using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [BurstCompile]
    public partial struct ProjectileSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            if(!SystemAPI.TryGetSingletonEntity<ProjectileConfig>(out var config)) return;

            foreach (var (data, transform, entity) 
                     in SystemAPI.Query<RefRO<ProjectileData>, RefRW<LocalTransform>>().WithEntityAccess() )
            {
                float3 velocity = data.ValueRO.Direction * data.ValueRO.Speed;
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(data.ValueRO.Direction, math.up());
                transform.ValueRW.Position += velocity;
            }
        }
    }
}