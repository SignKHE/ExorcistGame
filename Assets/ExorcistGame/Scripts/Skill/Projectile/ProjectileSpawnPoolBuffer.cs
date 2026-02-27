using Unity.Entities;

namespace ExorcistGame.Skill
{
    public struct ProjectileSpawnPoolBuffer : IBufferElementData
    {
        public Entity ProjectileEntity;
    }
}