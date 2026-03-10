using ExorcistGame.Character.State;
using ExorcistGame.Skill;
using ExorcistGame.VisualSync;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Character.Player
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float speed = 5f;
        public GameObject projectilePrefab;
        
        private class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerTag());
                AddComponent(entity, new CharacterTag());
                AddComponent(entity, new VisualSyncTag());
                AddComponent(entity, new MovementData() {MoveDirection = float2.zero});
                AddComponent(entity, new AttackData() {
                    DetectionRange = 50f, 
                    AttackRange = 20f, 
                    AttackTime = 1f, 
                    AttackTimer = 0f, 
                    ReloadTime = 1f, 
                    ReloadTimer = 0f
                });
                AddComponent(entity, new StateData() {IsInitialized = false});
                AddComponent(entity, new ProjectileSpawner(GetEntity(authoring.projectilePrefab, TransformUsageFlags.Dynamic), new ProjectileData(instigator:entity, damage:50f,speed:2f), poolSize:3));
            }
        }
    }
}
