using ExorcistGame.Character.State;
using ExorcistGame.Skill;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character.Player
{
    [BurstCompile]
    public partial struct PlayerAttackSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (attackData, movementData, transform, entity)
                     in SystemAPI.Query<RefRW<AttackData>, RefRO<MovementData>, RefRW<LocalTransform>>()
                         .WithAll<PlayerTag, AttackState>().WithEntityAccess())
            {
                Entity closestTarget = Entity.Null;
                float minDistance = attackData.ValueRO.DetectionRange;
                float3 playerPos = transform.ValueRO.Position;
                float3 movement = movementData.ValueRO.Direction;
                float distance = 0f;
                float deltaTime = SystemAPI.Time.DeltaTime;
                
                // 만약 움직임이 있다면 Move 상태로 변경
                if (math.lengthsq(movement) > float.Epsilon)
                {
                    attackData.ValueRW.Reset();
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    ecb.SetComponentEnabled<MoveState>(entity, true);
                    
                    continue;
                }

                // 타겟이 없다면 Idle로 변경
                if (attackData.ValueRO.Target == Entity.Null)
                {
                    attackData.ValueRW.Reset();
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                }
                
                float3 targetPos = SystemAPI.GetComponent<LocalTransform>(attackData.ValueRO.Target).Position;
                targetPos.y = 0f;
                playerPos.y = 0f;
                float3 targetDirection = math.normalizesafe(targetPos - playerPos);
                transform.ValueRW.Rotation = quaternion.LookRotationSafe(targetDirection, math.up());
                        
                distance = math.distance(playerPos, targetPos);

                if (distance < attackData.ValueRO.AttackRange)
                {
                    if (attackData.ValueRO.ReloadTimer < attackData.ValueRO.ReloadTime)
                    {
                        attackData.ValueRW.ReloadTimer += deltaTime;
                    }
                    else if (attackData.ValueRO.AttackTimer < attackData.ValueRO.AttackTime)
                    {
                        if (attackData.ValueRO.AttackTimer == 0f)
                        {
                            // 여기서 공격
                            // UnityEngine.Debug.Log($"플레이어 공격");
                            var spawnBuffer = SystemAPI.GetBuffer<ProjectileSpawnRequestBuffer>(entity);

                            spawnBuffer.Add(new ProjectileSpawnRequestBuffer()
                            { 
                                SpawnLocation = transform.ValueRO.Position + new float3(0f, 1f, 0f),
                                Direction = targetDirection
                            });
                        }
                        attackData.ValueRW.AttackTimer += deltaTime;
                    }
                    else
                    {
                        //UnityEngine.Debug.Log($"플레이어 공격 초기화");
                        attackData.ValueRW.Reset();
                    }
                }
                else
                {
                    //UnityEngine.Debug.Log($"플레이어 공격 초기화");
                    attackData.ValueRW.Reset();
                    
                }
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}