using ExorcistGame.Character;
using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "BasicAttackData", menuName = "ExorcistGame/BasicAttack")]
    public class BasicAttackSO : ScriptableObject
    {
        public AttackData AttackData;
    }
}
