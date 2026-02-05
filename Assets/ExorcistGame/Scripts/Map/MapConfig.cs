using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Map
{
    public struct MapConfig : IComponentData
    {
        /// <summary>
        /// 청크의 사이즈
        /// </summary>
        public float ChunkSize;
        /// <summary>
        /// 청크가 존재할 수 있는 플레이어와의 거리
        /// </summary>
        public int ViewDistance;
        /// <summary>
        /// 청크 엔티티 프리팹
        /// </summary>
        public Entity ChunkPrefab;
    }
}
