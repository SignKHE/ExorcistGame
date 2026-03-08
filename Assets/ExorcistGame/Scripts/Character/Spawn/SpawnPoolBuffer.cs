using Unity.Entities;

namespace ExorcistGame.Character.Spawn
{
    public struct SpawnPoolBuffer : IBufferElementData
    {
        public Entity LoadedEntity;
    }
}