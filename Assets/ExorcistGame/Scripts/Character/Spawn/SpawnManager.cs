using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Character.Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        private EntityManager _entityManager;
        private Entity _requestEntity;

        void Start()
        {
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        /// <summary>
        /// 스폰 요청 함수
        /// </summary>
        /// <param name="count">스폰 갯수</param>
        /// <param name="radius">스폰 반경</param>
        public void RequestSpawn(int count, float radius)
        {
            var query = _entityManager.CreateEntityQuery(typeof(SpawnRequestData));
            
            if(query.IsEmpty) return;
            
            _requestEntity = query.GetSingletonEntity();
            
            var requestBuffers = _entityManager.GetBuffer<SpawnRequestData>(_requestEntity);

            requestBuffers.Add( new SpawnRequestData()
            {
                Count = count,
                Radius = radius
            });
        }
    }
}
