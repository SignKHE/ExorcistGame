using UnityEngine;

namespace ExorcistGame.Data
{
    [CreateAssetMenu(fileName = "MovementData", menuName = "ExorcistGame/Movement")]
    public class MovementSO : ScriptableObject
    {
        public MovementData MovementData;
    }
}
