using System;
using ExorcistGame.Character;
using ExorcistGame.Character.Monster;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ExorcistGame.VisualSync
{
    /// <summary>
    /// 비주얼 동기화가 필요한 엔티티가 생성되면 비주얼 담당 게임 오브젝트를 스폰처리 해주는 시스템
    /// </summary>
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class VisualSpawnSystem : SystemBase
    {
        [Obsolete("Obsolete")]
        protected override void OnUpdate()
        {
            NativeList<Entity> toSpawn = new NativeList<Entity>(Allocator.TempJob);
            NativeList<Entity> toDespawn = new NativeList<Entity>(Allocator.TempJob);
            
            // VisualSyncTag가 있는데 VisualData가 없는 엔티티 검색
            // 스폰 JOB에 추가
            Entities
                .WithAll<VisualSyncData>()
                .WithNone<VisualData>()
                .ForEach((Entity entity) =>
                {
                    toSpawn.Add(entity);
                })
                .WithBurst()
                .Run();

            // VisualSyncTag는 있는데 LocalTransform 컴포넌트가 없는 엔티티 == 엔티티가 파괴된 상태
            // 디스폰 JOB에 추가
            Entities
                .WithAll<VisualData, Disabled>()
                .ForEach((Entity entity) =>
                {
                    toDespawn.Add(entity);
                })
                .WithBurst()
                .Run();

            // 메인 스레드 처리
            
            // Spawn 처리
            foreach (var entity in toSpawn)
            {
                 LocalTransform transform = SystemAPI.GetComponent<LocalTransform>(entity);
                 EVisualObject characterType = SystemAPI.GetComponentRO<VisualSyncData>(entity).ValueRO.VisualObject;
                 GameObject visualGameObject = VisualPoolManager.Instance.GetVisual(characterType);
                 visualGameObject.transform.position = transform.Position;
                 visualGameObject.SetActive(true);
                 EntityManager.AddComponentData(entity, new VisualData() {VisualObject = visualGameObject});
            }

            // Despawn 처리
            foreach (var entity in toDespawn)
            {
                EVisualObject characterType = SystemAPI.GetComponentRO<VisualSyncData>(entity).ValueRO.VisualObject;
                VisualData visualData = EntityManager.GetComponentData<VisualData>(entity);
                VisualPoolManager.Instance.ReturnVisual(visualData.VisualObject, characterType);
                
                EntityManager.RemoveComponent<VisualData>(entity);
            }

            toSpawn.Dispose();
            toDespawn.Dispose();
        }
    }
}