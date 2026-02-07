using ExorcistGame.VisualSync;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Spawn
{
    [BurstCompile]
    public partial struct SpawnProcessSystem : ISystem
    {
        private Random _random;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            _random = new Unity.Mathematics.Random(39);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.TryGetSingleton<SpawnConfig>(out SpawnConfig config)) return;

            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (request, entity) in SystemAPI.Query<RefRO<SpawnRequestData>>().WithEntityAccess())
            {
                for (int i = 0; i < request.ValueRO.Count; i++)
                {
                    Entity newMonster = ecb.Instantiate(config.MonsterPrefab);
                    float3 randomOffset = _random.NextFloat3Direction() * _random.NextFloat(0, request.ValueRO.Radius);
                    randomOffset.y = 0;
                    float3 finalPos = request.ValueRO.Position + randomOffset;
                    ecb.AddComponent(newMonster, LocalTransform.FromPosition(finalPos));
                    ecb.AddComponent(newMonster, new VisualSyncTag());
                }
                ecb.DestroyEntity(entity);
            }
        }
    }
}