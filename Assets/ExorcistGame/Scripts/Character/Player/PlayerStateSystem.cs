using ExorcistGame.Character.State;
using ExorcistGame.Skill;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Character
{
    [BurstCompile]
    public partial struct PlayerStateSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (playerState, movementData, playerTransform, attackData, playerEntity) 
                     in SystemAPI.Query<RefRW<StateData>, RefRO<MovementData>, RefRW<LocalTransform>, RefRW<AttackData>>().WithAll<PlayerTag>().WithEntityAccess())
            {
                float2 movement = movementData.ValueRO.MoveDirection;
                float3 playerPos = playerTransform.ValueRO.Position;
                float distance = 0f;
                float deltaTime = SystemAPI.Time.DeltaTime;
                
                // 만약 움직임이 있다면 Move 상태로 변경
                if (math.lengthsq(movement) > float.Epsilon)
                {
                    playerState.ValueRW.State = EState.Move;
                }
                // 만약 움직임이 없는데 Move 상태라면 Idle 상태로 변경
                else if (playerState.ValueRO.State == EState.Move)
                {
                    playerState.ValueRW.State = EState.Idle;
                }
                
                switch (playerState.ValueRO.State)
                {
                    case EState.Idle:
                        // 근처의 적 유닛 찾기
                        Entity closestTarget = Entity.Null;
                        float minDistance = attackData.ValueRO.DetectionRange;

                        foreach (var (monsterTransform, monsterEntity) 
                                 in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<MonsterData>().WithEntityAccess())
                        {
                            distance = math.distancesq(playerPos, monsterTransform.ValueRO.Position);

                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                closestTarget = monsterEntity;
                            }
                        }

                        // 가까운 적이 있다면 타겟으로 설정
                        if (closestTarget != Entity.Null)
                        {
                            attackData.ValueRW.Target = closestTarget;
                            playerState.ValueRW.State = EState.Attack;
                        }
                        
                        break;
                    case EState.Move:
                        // 움직임
                        
                        break;
                    case EState.Attack:
                        // 찾은 적유닛 바라보기 & 공격
                        if (attackData.ValueRO.Target == Entity.Null)
                        {
                            playerState.ValueRW.State = EState.Idle;
                            break;
                        }
                        float3 targetPos = SystemAPI.GetComponent<LocalTransform>(attackData.ValueRO.Target).Position;
                        targetPos.y = 0f;
                        playerPos.y = 0f;
                        float3 targetDirection = math.normalizesafe(targetPos - playerPos);
                        playerTransform.ValueRW.Rotation = quaternion.LookRotationSafe(targetDirection, math.up());
                        
                        distance = math.distance(playerPos, targetPos);

                        if (distance < attackData.ValueRO.AttackRange)
                        {
                            if (attackData.ValueRO.ReloadTimer < attackData.ValueRO.ReloadTime)
                            {
                                attackData.ValueRW.ReloadTimer += deltaTime;
                            }
                            else if (attackData.ValueRO.AttackTimer == 0f)
                            {
                                // 여기서 공격
                                UnityEngine.Debug.Log($"플레이어 공격");
                                DynamicBuffer<ProjectileSpawnBuffer> spawnBuffer = SystemAPI.GetBuffer<ProjectileSpawnBuffer>(playerEntity);

                                spawnBuffer.Add(new ProjectileSpawnBuffer()
                                {
                                    SpawnLocation = playerTransform.ValueRO.Position + new float3(0f, 1f, 0f),
                                    Data = new ProjectileData(damage:20f,direction:targetDirection,speed:10f,lifeTime:10f,playerEntity)
                                });
                            }
                            else if (attackData.ValueRO.AttackTimer < attackData.ValueRO.AttackTime)
                            {
                                attackData.ValueRW.AttackTimer += deltaTime;
                            }
                            else
                            {
                                UnityEngine.Debug.Log($"플레이어 공격 초기화");
                                attackData.ValueRW.AttackTimer = 0;
                                attackData.ValueRW.ReloadTimer = 0;
                            }
                        }
                        else
                        {
                            UnityEngine.Debug.Log($"플레이어 공격 초기화");
                            attackData.ValueRW.AttackTimer = 0;
                            attackData.ValueRW.ReloadTimer = 0;
                        }
                        
                        break;
                    default:
                        return;
                }
            }
        }
    }
}