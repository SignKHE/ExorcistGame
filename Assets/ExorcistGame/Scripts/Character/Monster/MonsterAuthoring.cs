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
                
                AddComponent(entity, new VisualSyncData() {VisualObject = EVisualObject.Monster});
                AddComponent(entity, new MonsterData());
                AddComponent(entity, new CharacterTag());
                AddComponent(entity, new HPData(hp:100f));
                
                AddComponent(entity, authoring.MonsterData.MovementData.Data.Reset());
                AddComponent(entity, authoring.MonsterData.BasicAttackData.Data.Reset());
                AddComponent(entity, new StateData() {IsInitialized = false});
                AddComponent(entity, authoring.MonsterData.SeekerData.Data);
            }
        }
    }
}