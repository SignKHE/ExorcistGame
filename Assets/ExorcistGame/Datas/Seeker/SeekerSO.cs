using ExorcistGame.Seeker;
using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "SeekerData", menuName = "ExorcistGame/Seeker")]
    public class SeekerSO : ScriptableObject
    {
        public SeekerData SeekerData;
    }
}