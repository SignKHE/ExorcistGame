using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Spawn
{
    public class SpawnConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject monsterPrefab;
        
        private class SpawnConfigBaker : Baker<SpawnConfigAuthoring>
        {
            public override void Bake(SpawnConfigAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new SpawnConfig
                {
                    MonsterPrefab = GetEntity(authoring.monsterPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}
