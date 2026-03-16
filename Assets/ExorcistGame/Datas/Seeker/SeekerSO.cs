using ExorcistGame.Seeker;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "SeekerData", menuName = "ExorcistGame/Seeker")]
    public class SeekerSO : ScriptableObject
    {
        [FormerlySerializedAs("SeekerData")] public SeekerData Data;
    }
}