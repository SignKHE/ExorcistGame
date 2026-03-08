using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Character.Spawn
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
                Entity spawnConfigEntity = GetEntity(TransformUsageFlags.None);

                AddComponent(spawnConfigEntity, new SpawnConfig
                {
                    Radius = authoring.radius,
                    SpawnMax = authoring.spawnMax
                });
                
                Entity spawnPoolEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, "SpawnPoolEntity");
                AddComponent(spawnPoolEntity, new SpawnPoolData
                {
                    MonsterPrefab = GetEntity(authoring.monsterPrefab, TransformUsageFlags.Dynamic),
                });
                AddBuffer<SpawnPoolBuffer>(spawnPoolEntity);
                AddBuffer<SpawnRequestData>(spawnPoolEntity);
            }
        }
    }
}
