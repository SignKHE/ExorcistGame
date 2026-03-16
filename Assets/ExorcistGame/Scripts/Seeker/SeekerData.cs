using System;
using Unity.Entities;
using UnityEngine;

namespace ExorcistGame.Seeker
{
    [Serializable]
    public struct SeekerData :  IComponentData, IEnableableComponent
    {
        [Label("탐색 반경")]
        public float Range;
        [Label("탐색 대상")]
        public ETargetType TargetType;
    }

    public enum ETargetType : byte
    {
        [InspectorName("플레이어")]
        Player,
        [InspectorName("몬스터")]
        Monster
    }
}