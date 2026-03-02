using ExorcistGame.Character.State;
using ExorcistGame.Damage;
using ExorcistGame.Skill;
using ExorcistGame.VisualSync;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ExorcistGame.Character.Monster
{
    public class MonsterAuthoring : MonoBehaviour
    {
        public GameObject projectilePrefab;
        private class MonsterBaker : Baker<MonsterAuthoring>
        {
            public override void Bake(MonsterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new VisualSyncTag());
                AddComponent(entity, new MonsterData());
                AddComponent(entity, new CharacterTag());
                AddComponent(entity, new HPData(hp:100f));
                
                AddComponent(entity, new MovementData() {MoveDirection = float2.zero});
                AddComponent(entity, new AttackData() {
                    DetectionRange = 30f, 
                    AttackRange = 10f, 
                    AttackTime = 1f, 
                    AttackTimer = 0f, 
                    ReloadTime = 1f, 
                    ReloadTimer = 0f
                });
                AddComponent(entity, new StateData() {IsInitialized = false});
                AddComponent(entity, new ProjectileSpawner(GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic)));
                AddBuffer<ProjectileSpawnRequestBuffer>(entity);
                AddBuffer<ProjectileSpawnPoolBuffer>(entity);
            }
        }
    }
}