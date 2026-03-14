using Unity.Entities;

namespace ExorcistGame.Seeker
{
    [InternalBufferCapacity(8)] // 8개까지 기본 할당
    public struct FoundEntityBuffer : IBufferElementData
    {
        public Entity Value;
    }
}