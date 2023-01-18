using System;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

namespace Utilities.Singletons
{
    [RequireComponent(typeof(Multiplayer), typeof(Owner), typeof(Alteruna.Spawner))]
    public class ExtendedSpawner : Alteruna.Spawner
    {
        [SerializeField] private List<SpawnerEntry> _items;
        private int _spawnerID;

        private bool CheckOwnerIDMatch(int id)
        {
            return _spawnerID == id;
        }

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

        protected void Awake()
        {
            _spawnerID = this.GetComponent<Owner>().ID = this.GetComponent<Multiplayer>().Me.Index;

            int LastIndex = SpawnableObjects.Count;

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].index = LastIndex + i;
                SpawnableObjects.Add(_items[i].prefab);
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
                output = Spawn(_items[index].index, position, rotation, scale);

                if (output.GetComponent<Owner>() == null)
                {
                    output.AddComponent<Owner>().ID = _spawnerID;
                }
            }

            return output;
        }

        public void RequestDespawn(int requesterID, Owner despawnTarget)
        {
            if (_spawnerID != requesterID)
            {
                return;
            }

            Despawn(despawnTarget.gameObject);
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