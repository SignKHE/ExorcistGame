using ExorcistGame.Character.State;
using ExorcistGame.Damage;
using ExorcistGame.Data;
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
        public CharacterSO MonsterData;
        private class MonsterBaker : Baker<MonsterAuthoring>
        {
            public override void Bake(MonsterAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new VisualSyncTag());
                AddComponent(entity, new MonsterData());
                AddComponent(entity, new CharacterTag());
                AddComponent(entity, new HPData(hp:100f));
                
                AddComponent(entity, authoring.MonsterData.MovementData.MovementData);
                AddComponent(entity, authoring.MonsterData.BasicAttackData.AttackData);
                AddComponent(entity, new StateData() {IsInitialized = false});
                AddComponent(entity, new ProjectileSpawner(
                    projectilePrefab: GetEntity(authoring.MonsterData.ProjectileSpawnerData.ProjectilePrefab, TransformUsageFlags.Dynamic), 
                    poolSize: authoring.MonsterData.ProjectileSpawnerData.ProjectileSpawnerData.PoolSize,
                    projectileSpeed: authoring.MonsterData.ProjectileSpawnerData.ProjectileSpawnerData.ProjectileSpeed,
                    projectileDataPreset: authoring.MonsterData.ProjectileSpawnerData.ProjectileSpawnerData.ProjectileDataPreset.SetInstigator(entity)
                    ));
                AddComponent(entity, authoring.MonsterData.SeekerData.SeekerData);
            }
        }
    }
}