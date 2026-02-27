using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill
{
    public class ProjectileDataAuthoring : MonoBehaviour
    {
        [SerializeField]
        private bool isPlayer = false;
        private class ProjectileDataBaker : Baker<ProjectileDataAuthoring>
        {
            public override void Bake(ProjectileDataAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, ProjectileData.Empty);
                if (authoring.isPlayer)
                {
                    AddComponent(entity, new PlayerTag());
                }
                SetComponentEnabled<ProjectileData>(entity,false);
            }
        }
    }
}