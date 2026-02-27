using ExorcistGame.Damage;
using Unity.Entities;

namespace ExorcistGame.Skill
{
    public struct HitBuffer : IBufferElementData
    {
        public Entity Target;
        public DamageBufferElement DamageData;
    }
}