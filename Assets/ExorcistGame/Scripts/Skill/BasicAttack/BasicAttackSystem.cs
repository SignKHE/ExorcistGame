using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Skill.BasicAttack
{
    /// <summary>
    /// 기본 공격 시스템
    /// </summary>
    [BurstCompile]
    public partial struct BasicAttackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            
            foreach (var (requests, transform , entity) 
                     in SystemAPI.Query<DynamicBuffer<SkillRequestBuffer>, RefRO<LocalToWorld>>()
                         .WithAll<BasicAttackTag>()
                         .WithEntityAccess())
            {

                foreach (var request in requests)
                {
                    float3 playerPos = transform.ValueRO.Position;
                    float3 targetPos = request.TargetPosition;
                    float3 targetDirection = math.normalizesafe(targetPos - playerPos);
                    
                    ecb.AppendToBuffer( entity, new ProjectileSpawnRequestBuffer()
                    { 
                        SpawnLocation = playerPos + new float3(0f, 1f, 0f),
                        Direction = targetDirection
                    });
                }
                
                requests.Clear();
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}