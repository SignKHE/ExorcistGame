using ExorcistGame.Character.State;
using ExorcistGame.Damage;
using ExorcistGame.Seeker;
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
                
                AddComponent(entity, new MovementData() {Direction = float3.zero, Speed = 1f});
                AddComponent(entity, new AttackData() {
                    AttackRange = 5f, 
                    AttackTime = 1f, 
                    AttackTimer = 0f, 
                    ReloadTime = 1f, 
                    ReloadTimer = 0f
                });
                AddComponent(entity, new StateData() {IsInitialized = false});
                AddComponent(entity, new ProjectileSpawner(
                    GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic), 
                    new ProjectileData(instigator:entity), 
                    poolSize:3
                    ));
                AddComponent(entity, new SeekerData()
                {
                    Range = 30f,
                    TargetType = ETargetType.Player
                });
            }
        }
    }
}