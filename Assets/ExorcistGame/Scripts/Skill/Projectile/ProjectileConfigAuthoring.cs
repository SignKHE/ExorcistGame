using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill
{
    public class ProjectileConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject projectilePrefab;
        private class ProjectileConfigBaker : Baker<ProjectileConfigAuthoring>
        {
            public override void Bake(ProjectileConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new ProjectileConfig()
                {
                    ProjectilePrefab = GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}