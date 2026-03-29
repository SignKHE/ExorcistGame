using ExorcistGame.VisualSync;
using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Skill
{
    public class ProjectileDataAuthoring : MonoBehaviour
    {
        [SerializeField] private bool isVisual = false;
        [SerializeField]
        private EVisualObject visualObject;
        private class ProjectileDataBaker : Baker<ProjectileDataAuthoring>
        {
            public override void Bake(ProjectileDataAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                if (authoring.isVisual)
                {
                    AddComponent(entity, new VisualSyncData() { VisualObject = authoring.visualObject });
                }
            }
        }
    }
}