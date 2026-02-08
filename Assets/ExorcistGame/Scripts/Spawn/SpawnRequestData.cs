using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Spawn
{
    public struct SpawnRequestData : IComponentData
    {
        /// <summary>
        /// 스폰할 몬스터의 갯수
        /// </summary>
        public int Count;
        /// <summary>
        /// 스폰 반경
        /// </summary>
        public float Radius;
    }
}