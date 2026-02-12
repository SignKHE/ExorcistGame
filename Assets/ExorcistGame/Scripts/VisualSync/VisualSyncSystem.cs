using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace ExorcistGame.VisualSync
{
    /// <summary>
    /// 비주얼 동기화 시스템
    /// </summary>
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial class VisualSyncSystem : SystemBase
    {
        [Obsolete("Obsolete")]
        protected override void OnUpdate()
        {
            NativeList<Entity> toSync = new NativeList<Entity>(Allocator.TempJob);
            
            Entities
                .WithAll<VisualData, LocalTransform>()
                .ForEach((Entity entity) =>
                {
                    toSync.Add(entity);
                })
                .WithBurst()
                .Run();

            foreach (var entity in toSync)
            {
                LocalTransform transform = SystemAPI.GetComponent<LocalTransform>(entity);
                GameObject visualGameObject = EntityManager.GetComponentData<VisualData>(entity).VisualObject;
                visualGameObject.transform.position = transform.Position;
                visualGameObject.transform.rotation = transform.Rotation;
            }
            
            toSync.Dispose();
        }
    }
}