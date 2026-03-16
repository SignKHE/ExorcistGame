using UnityEngine;

namespace ExorcistGame.Data
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ExorcistGame/Character")]
    public class CharacterSO : ScriptableObject
    {
        public MovementSO MovementData;
        public BasicAttackSO BasicAttackData;
        public ProjectileSpawnerSO ProjectileSpawnerData;
        public SeekerSO SeekerData;
    }
}
