using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Map
{
    public struct MapConfig : IComponentData
    {
        /// <summary>
        /// 맵 엔티티 프리팹
        /// </summary>
        public Entity MapPrefab;
    }
}
