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
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (attackData, movementData, transform, entity)
                     in SystemAPI.Query<RefRW<AttackData>, RefRW<MovementData>, RefRW<LocalTransform>>()
                         .WithAll<PlayerTag, AttackState>().WithEntityAccess())
            {
                float3 playerPos = transform.ValueRO.Position;
                float3 movement = movementData.ValueRO.Direction;
                float distanceSq = 0f;
                
                // 만약 움직임이 있다면 Move 상태로 변경
                if (math.lengthsq(movement) > float.Epsilon)
                {
                    attackData.ValueRW.Reset();
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    ecb.SetComponentEnabled<MoveState>(entity, true);
                    
                    continue;
                }
                
                // 타겟이 존재하는지 확인
                if (!transformLookup.TryGetComponent(attackData.ValueRO.Target, out LocalTransform targetTransform))
                {
                    attackData.ValueRW.Target = Entity.Null;
                }

                // 타겟이 없다면 Idle로 변경
                if (attackData.ValueRO.Target == Entity.Null)
                {
                    attackData.ValueRW.Reset();
                    // 상태 변경 예약
                    ecb.SetComponentEnabled<AttackState>(entity, false);
                    ecb.SetComponentEnabled<IdleState>(entity, true);
                    continue;
                }
                
                
                
                float3 targetPos = targetTransform.Position;
                float3 targetDirection = math.normalizesafe(targetPos - playerPos);
                movementData.ValueRW.Direction = targetDirection * 0.00001f;
                        
                distanceSq = math.distancesq(playerPos, targetPos);

                if (distanceSq < attackData.ValueRO.AttackRange * attackData.ValueRO.AttackRange)
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