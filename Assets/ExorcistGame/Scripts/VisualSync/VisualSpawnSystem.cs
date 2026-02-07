using System;
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
                .WithAll<VisualSyncTag>()
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
                .WithAll<VisualData>()
                .WithNone<LocalTransform>()
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
                 GameObject visualGameObject = CharacterPoolManager.Instance.GetCharacter(ECharacterType.Monster);
                 visualGameObject.transform.position = transform.Position;
                 EntityManager.AddComponentData(entity, new VisualData() {VisualObject = visualGameObject});
                 EntityManager.AddComponentData(entity, new VisualSyncTag());
            }

            // Despawn 처리
            foreach (var entity in toDespawn)
            {
                VisualData visualData = EntityManager.GetComponentData<VisualData>(entity);
                CharacterPoolManager.Instance.ReturnCharacter(visualData.VisualObject, ECharacterType.Monster);
                
                // Cleanup 제거 -> 엔티티 완전 소멸
                EntityManager.RemoveComponent<VisualData>(entity);
            }

            toSpawn.Dispose();
            toDespawn.Dispose();
        }
    }
}