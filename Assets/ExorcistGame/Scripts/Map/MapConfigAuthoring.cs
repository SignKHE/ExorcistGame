using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Map
{
    public class MapConfigAuthoring  : MonoBehaviour
    {
        [SerializeField] private float chunkSize = 50f;
        [SerializeField] private int viewPrefab = 1;
        [SerializeField] private GameObject chunkPrefab;
        
        public class MapConfigBaker : Baker<MapConfigAuthoring>
        {
            public override void Bake(MapConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MapConfig
                {
                    ChunkSize = authoring.chunkSize,
                    ViewDistance = authoring.viewPrefab,
                    ChunkPrefab = GetEntity(authoring.chunkPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}