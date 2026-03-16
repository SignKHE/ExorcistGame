using ExorcistGame.Skill;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "ProjectileSpawnerData", menuName = "ExorcistGame/ProjectileSpawner")]
    public class ProjectileSpawnerSO : ScriptableObject
    {
        [FormerlySerializedAs("ProjectileSpawnerData")] public ProjectileSpawner Data;
        [Label("투사체 프리팹")]
        public GameObject ProjectilePrefab;
    }
}
