using UnityEngine;

namespace ExorcistGame.Data
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "ExorcistGame/Character")]
    public class CharacterSO : ScriptableObject
    {
        [Label("움직임 데이터")]
        public MovementSO MovementData;
        [Label("기본공격 데이터")]
        public BasicAttackSO BasicAttackData;
        [Label("탐색기 데이터")]
        public SeekerSO SeekerData;
    }
}
