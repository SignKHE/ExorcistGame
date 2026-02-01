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
    }
}