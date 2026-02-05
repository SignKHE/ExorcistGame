using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Map
{
    public class MapConfigAuthoring  : MonoBehaviour
    {
        [SerializeField]
        private GameObject mapPrefab;
        
        public class MapConfigBaker : Baker<MapConfigAuthoring>
        {
            public override void Bake(MapConfigAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new MapConfig
                {
                    MapPrefab = GetEntity(authoring.mapPrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}