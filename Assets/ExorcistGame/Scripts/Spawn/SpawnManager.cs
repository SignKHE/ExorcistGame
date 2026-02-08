using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace ExorcistGame.Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        private EntityManager _em;
        private EntityArchetype _requestArchetype;

        void Start()
        {
            _em = World.DefaultGameObjectInjectionWorld.EntityManager;
            _requestArchetype = _em.CreateArchetype(typeof(SpawnRequestData));
        }

        /// <summary>
        /// 스폰 요청 함수
        /// </summary>
        /// <param name="count">스폰 갯수</param>
        /// <param name="radius">스폰 반경</param>
        public void RequestSpawn(int count, float radius)
        {
            Entity requestEntity = _em.CreateEntity(_requestArchetype);
            
            _em.SetComponentData(requestEntity, new SpawnRequestData
            {
                Count = count,
                Radius = radius
            });
        }
    }
}
