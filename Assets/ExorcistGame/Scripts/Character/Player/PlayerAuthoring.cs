using ExorcistGame.VisualSync;
using UnityEngine;
using Unity.Entities;

namespace ExorcistGame.Character
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public float speed = 5f;
        
        private class PlayerBaker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity, new PlayerTag());
                AddComponent(entity, new VisualSyncTag());
                AddComponent(entity, new MovementData());
            }
        }
    }
}
