using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Map
{
    public class MapConfigAuthoring  : MonoBehaviour
    {
        [SerializeField]
        private GameObject chunkPrefab;
        
        public class MapConfigBaker : Baker<MapConfigAuthoring>
        {
            public override void Bake(MapConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MapConfig
                {
                    ChunkPrefab = GetEntity(authoring.chunkPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}