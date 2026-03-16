using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Character.Spawn
{
    public class SpawnConfigAuthoring : MonoBehaviour
    {
        [SerializeField] private SpawnConfigSO data;
        
        private class SpawnConfigBaker : Baker<SpawnConfigAuthoring>
        {
            public override void Bake(SpawnConfigAuthoring authoring)
            {
                Entity spawnConfigEntity = GetEntity(TransformUsageFlags.None);

                AddComponent(spawnConfigEntity, new SpawnConfig
                {
                    Radius = authoring.data.Radius,
                    SpawnMax = authoring.data.SpawnMax
                });
                
                Entity spawnPoolEntity = CreateAdditionalEntity(TransformUsageFlags.None, false, "SpawnPoolEntity");
                AddComponent(spawnPoolEntity, new SpawnPoolData
                {
                    MonsterPrefab = GetEntity(authoring.data.monsterPrefab, TransformUsageFlags.Dynamic),
                });
                AddBuffer<SpawnPoolBuffer>(spawnPoolEntity);
                AddBuffer<SpawnRequestData>(spawnPoolEntity);
            }
        }
    }
}
