using Unity.Entities;

namespace ExorcistGame.Character.Spawn
{
    public struct SpawnPoolData : IComponentData
    {
        /// <summary>
        /// 스폰하는 몬스터의 프리팹
        /// </summary>
        public Entity MonsterPrefab;
    }
}