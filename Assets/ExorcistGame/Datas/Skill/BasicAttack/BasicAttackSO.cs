using ExorcistGame.Character;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "BasicAttackData", menuName = "ExorcistGame/BasicAttack")]
    public class BasicAttackSO : ScriptableObject
    {
        [FormerlySerializedAs("AttackData")] public AttackData Data;
    }
}
