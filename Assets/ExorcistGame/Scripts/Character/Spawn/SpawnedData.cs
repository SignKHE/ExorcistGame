using Unity.Entities;

namespace ExorcistGame.Character.Spawn
{
    public struct SpawnedData : IComponentData
    {
        public Entity SpawnPool;
    }
}