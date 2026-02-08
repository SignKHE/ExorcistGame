using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Spawn
{
    public class SpawnConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject monsterPrefab;
        [SerializeField]
        private float radius = 15.0f;
        [SerializeField]
        private int spawnMax = 60;
        
        private class SpawnConfigBaker : Baker<SpawnConfigAuthoring>
        {
            public override void Bake(SpawnConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new SpawnConfig
                {
                    MonsterPrefab = GetEntity(authoring.monsterPrefab, TransformUsageFlags.Dynamic),
                    Radius = authoring.radius,
                    SpawnMax = authoring.spawnMax
                });
            }
        }
    }
}
