using System.ComponentModel;
using Unity.Entities;

namespace ExorcistGame.Spawn
{
    public struct SpawnConfig : IComponentData
    {
        /// <summary>
        /// 스폰하는 몬스터의 프리팹
        /// </summary>
        public Entity MonsterPrefab;
        /// <summary>
        /// 몬스터가 스폰하는 반경
        /// </summary>
        public float Radius;
        /// <summary>
        /// 최대 생성가능 수치
        /// </summary>
        public int SpawnMax;
    }
}