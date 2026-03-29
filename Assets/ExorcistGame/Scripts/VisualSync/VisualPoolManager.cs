using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame.VisualSync
{
    public class VisualPoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<VisualPool> visualPools = new List<VisualPool>();
        
        public static VisualPoolManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var visualPool in visualPools)
            {
                visualPool.Initialize();
            }
        }

        public GameObject GetVisual(EVisualObject type)
        {
            return visualPools.FirstOrDefault(visualPool => visualPool.VisualObjectType == type)?.GetVisual();
        }

        public void ReturnVisual(GameObject visualObject, EVisualObject type)
        {
            visualPools.FirstOrDefault(visualPool => visualPool.VisualObjectType == type)?.ReturnVisual(visualObject);
        }

        [Serializable]
        public class VisualPool
        {
            [SerializeField]
            private EVisualObject visualObjectType;
            [SerializeField]
            private int _poolSize = 100;
            /// <summary>
            /// 캐릭터 프리팹
            /// </summary>
            [SerializeField]
            private GameObject _visualPrefab;
            /// <summary>
            /// 캐릭터 풀 스택
            /// </summary>
            private readonly Stack<GameObject> _poolStack = new ();

            public EVisualObject VisualObjectType {get => visualObjectType;}
            public void Initialize()
            {
                for (int i = 0; i < _poolSize; i++)
                {
                    ReturnVisual(Instantiate(_visualPrefab));
                }
            }
            
            public GameObject GetVisual()
            {
                return _poolStack.Pop();
            }

            public void ReturnVisual(GameObject obj)
            {
                _poolStack.Push(obj);
                _poolStack.Peek().SetActive(false);
            }
        }
        
        
    }
}