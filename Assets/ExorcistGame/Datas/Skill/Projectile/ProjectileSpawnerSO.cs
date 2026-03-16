using ExorcistGame.Skill;
using UnityEngine;

namespace ExorcistGame
{
    [CreateAssetMenu(fileName = "ProjectileSpawnerData", menuName = "ExorcistGame/ProjectileSpawner")]
    public class ProjectileSpawnerSO : ScriptableObject
    {
        public ProjectileSpawner ProjectileSpawnerData;
        public GameObject ProjectilePrefab;
    }
}
