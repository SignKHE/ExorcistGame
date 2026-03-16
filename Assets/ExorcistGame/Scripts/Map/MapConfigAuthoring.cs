using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Map
{
    public class MapConfigAuthoring  : MonoBehaviour
    {
        [SerializeField] private MapConfigSO data;
        
        public class MapConfigBaker : Baker<MapConfigAuthoring>
        {
            public override void Bake(MapConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MapConfig
                {
                    ChunkSize = authoring.data.ChunkSize,
                    ViewDistance = authoring.data.ViewDistance,
                    ChunkPrefab = GetEntity(authoring.data.ChunkPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}