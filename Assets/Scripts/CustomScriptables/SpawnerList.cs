using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CustomScriptables
{
    [CreateAssetMenu(menuName = "Scriptables/SpawnerList", fileName = "SpawnerList")]
    public class SpawnerList : ScriptableObject
    {
        [SerializeField] private List<SpawnerEntry> _items;

        private int FindInList(string key)
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

        public GameObject SpawnByKey(string key, Vector3 position)
        {
            return SpawnByKey(key, position, quaternion.identity, Vector3.one);
        }

        public GameObject SpawnByKey(string key, Vector3 position, Quaternion rot)
        {
            return SpawnByKey(key, position, rot, Vector3.one);
        }

        public GameObject SpawnByKey(string key, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject output = null;

            int index = FindInList(key);

            if (index == -1)
            {
                Debug.LogError("Found no game object within list - returning null");
            }
            else
            {
                output = Utilities.Spawner.Instance.Spawn(_items[index].index, position, rotation, scale);
            }

            return output;
        }
    }

    [Serializable]
    public struct SpawnerEntry
    {
        public string key;
        public int index;
    }
}