using Unity.Entities;

namespace ExorcistGame.Skill
{
    /// <summary>
    /// 투사체 스포너의 투사체 풀 버퍼
    /// </summary>
    public struct ProjectileSpawnPoolBuffer : IBufferElementData
    {
        public Entity ProjectileEntity;
    }
}