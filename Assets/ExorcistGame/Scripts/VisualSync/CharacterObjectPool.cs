using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExorcistGame.VisualSync
{
    public class CharacterPoolManager : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterPool> characterPools = new List<CharacterPool>();
        
        public static CharacterPoolManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var characterPool in characterPools)
            {
                characterPool.Initialize();
            }
        }

        public GameObject GetCharacter(ECharacterType type)
        {
            return characterPools.FirstOrDefault(characterPool => characterPool.CharacterType == type)?.GetCharacter();
        }

        public void ReturnCharacter(GameObject character, ECharacterType type)
        {
            characterPools.FirstOrDefault(characterPool => characterPool.CharacterType == type)?.ReturnCharacter(character);
        }

        [Serializable]
        public class CharacterPool
        {
            [SerializeField]
            private ECharacterType characterType;
            [SerializeField]
            private int _poolSize = 100;
            /// <summary>
            /// 캐릭터 프리팹
            /// </summary>
            [SerializeField]
            private GameObject _characterPrefab;
            /// <summary>
            /// 캐릭터 풀 스택
            /// </summary>
            private readonly Stack<GameObject> _poolStack = new ();

            public ECharacterType CharacterType {get => characterType;}
            public void Initialize()
            {
                for (int i = 0; i < _poolSize; i++)
                {
                    ReturnCharacter(Instantiate(_characterPrefab));
                }
            }
            
            public GameObject GetCharacter()
            {
                return _poolStack.Pop();
            }

            public void ReturnCharacter(GameObject obj)
            {
                _poolStack.Push(obj);
                _poolStack.Peek().SetActive(false);
            }
        }
        
        
    }
}