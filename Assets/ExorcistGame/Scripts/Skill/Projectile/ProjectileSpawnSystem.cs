using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace ExorcistGame.Skill
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [BurstCompile]
    public partial struct ProjectileSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if(!SystemAPI.TryGetSingleton<ProjectileConfig>(out var config)) return;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            
            var job = new SpawnProjectileJob
            {
                ECB = ecb,
                Config = config,
            };

            job.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    public partial struct SpawnProjectileJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public ProjectileConfig Config;
        
        public void Execute(Entity spawnerEntity, [EntityIndexInQuery] int sortKey, ref DynamicBuffer<ProjectileSpawnBuffer> spawnBuffer)
        {
            for (int i = 0; i < spawnBuffer.Length; i++)
            {
                var request = spawnBuffer[i];
                Entity spawnedProjectile = ECB.Instantiate(sortKey, Config.ProjectilePrefab);
                ECB.AddComponent(sortKey, spawnedProjectile, new LocalTransform(){Position = request.SpawnLocation});
                ECB.AddComponent(sortKey, spawnedProjectile, request.Data);
            }
            ECB.RemoveComponent<ProjectileSpawnBuffer>(sortKey, spawnerEntity);
        }
    }
}