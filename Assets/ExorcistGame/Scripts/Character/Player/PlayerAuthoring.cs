using ExorcistGame.Character.State;
using ExorcistGame.Data;
using ExorcistGame.Skill;
using ExorcistGame.VisualSync;
using UnityEngine;
using Unity.Entities;

namespace ExorcistGame.Character.Player
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public CharacterSO playerData;
        
        private class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerTag());
                AddComponent(entity, new CharacterTag());
                AddComponent(entity, new VisualSyncTag());
                AddComponent(entity, authoring.playerData.MovementData.Data.Reset());
                AddComponent(entity, authoring.playerData.BasicAttackData.Data.Reset());
                AddComponent(entity, new StateData() {IsInitialized = false});
                AddComponent(entity, new ProjectileSpawner()
                {
                    PoolSize = authoring.playerData.ProjectileSpawnerData.Data.PoolSize,
                    ProjectilePrefab = GetEntity(authoring.playerData.ProjectileSpawnerData.ProjectilePrefab, TransformUsageFlags.Dynamic),
                    ProjectileSpeed = authoring.playerData.ProjectileSpawnerData.Data.ProjectileSpeed,
                    ProjectileDataPreset = (authoring.playerData.ProjectileSpawnerData.Data.ProjectileDataPreset).SetInstigator(entity).Reset()
                });
                AddComponent(entity, authoring.playerData.SeekerData.Data);
            }
        }
    }
}
