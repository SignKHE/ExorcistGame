using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame.Data
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "ExorcistGame/Movement")]
    public class MovementSO : ScriptableObject
    {
        [FormerlySerializedAs("MovementData")] public MovementData Data;
    }
}
