using Unity.Entities;

namespace ExorcistGame.VisualSync
{
    /// <summary>
    /// 비주얼 동기화하는 엔티티 구별용 태그
    /// </summary>
    public struct VisualSyncData : IComponentData
    {
        public EVisualObject VisualObject;
    }
}