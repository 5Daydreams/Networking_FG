using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Singletons
{
    [RequireComponent(typeof(Alteruna.Spawner))]
    public class Spawner : Singleton<Alteruna.Spawner>
    {
        [SerializeField] private List<SpawnerEntry> _items;

        private int FindIndexInList(string key)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                bool nameFound = _items[i].key == key;
                if (nameFound)
                {
                    return _items[i].index;
                }
            }

            return -1;
        }

        protected override void Awake()
        {
            base.Awake();

            int LastIndex = Instance.SpawnableObjects.Count;

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].index = LastIndex + i;
                Instance.SpawnableObjects.Add(_items[i].prefab);
            }
        }

        public GameObject SpawnByKey(string key, Vector3 position)
        {
            return SpawnByKey(key, position, Quaternion.identity, Vector3.one);
        }

        public GameObject SpawnByKey(string key, Vector3 position, Quaternion rot)
        {
            return SpawnByKey(key, position, rot, Vector3.one);
        }

        public GameObject SpawnByKey(string key, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject output = null;

            int index = FindIndexInList(key);

            if (index == -1)
            {
                Debug.LogError("Found no game object within list - returning null");
            }
            else
            {
                output = Spawner.Instance.Spawn(_items[index].index, position, rotation, scale);
            }

            return output;
        }
    }
    
    [Serializable]
    public class SpawnerEntry
    {
        public string key;
        public GameObject prefab;
        [HideInInspector] public int index;
    }
}