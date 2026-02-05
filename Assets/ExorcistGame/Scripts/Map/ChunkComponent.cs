using Unity.Entities;
using Unity.Mathematics;

namespace ExorcistGame.Map
{
    public struct ChunkComponent : IComponentData
    {
        public int2 Coordinate;
    }
}