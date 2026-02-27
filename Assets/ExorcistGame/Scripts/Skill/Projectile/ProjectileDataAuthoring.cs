using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill
{
    public class ProjectileDataAuthoring : MonoBehaviour
    {
        private class ProjectileDataBaker : Baker<ProjectileDataAuthoring>
        {
            public override void Bake(ProjectileDataAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, ProjectileData.Empty);
                SetComponentEnabled<ProjectileData>(entity,false);
            }
        }
    }
}