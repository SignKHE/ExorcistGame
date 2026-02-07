using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.VisualSync
{
    /// <summary>
    /// 비주얼 동기화를 위해 필요한 정보를 가지는 컴포넌트
    /// </summary>
    public class VisualData : ICleanupComponentData
    {
        public GameObject VisualObject;
        public Animator Animator;
    }
}