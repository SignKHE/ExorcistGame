using ExorcistGame.Skill;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "ProjectileSpawnerData", menuName = "ExorcistGame/ProjectileSpawner")]
    public class ProjectileSpawnerSO : ScriptableObject
    {
        [FormerlySerializedAs("ProjectileSpawnerData")] public ProjectileSpawner Data;
        public GameObject ProjectilePrefab;
    }
}
