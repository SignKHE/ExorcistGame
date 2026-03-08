using Unity.Entities;

namespace ExorcistGame.Character.Spawn
{
    public struct SpawnConfig : IComponentData
    {
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