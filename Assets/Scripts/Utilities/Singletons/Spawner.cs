using CustomScriptables;
using UnityEngine;

namespace Utilities.Singletons
{
    [RequireComponent(typeof(Alteruna.Spawner))]
    public class Spawner : Singleton<Alteruna.Spawner>
    {
        [SerializeField] private SpawnerList _items;

        protected override void Awake()
        {
            base.Awake();

            int LastIndex = Instance.SpawnableObjects.Count;

            for (int i = 0; i < _items.List.Count; i++)
            {
                _items.List[i].index = LastIndex + i;
                Instance.SpawnableObjects.Add(_items.List[i].prefab);
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

            int index = _items.FindIndexInList(key);

            if (index == -1)
            {
                Debug.LogError("Found no game object within list - returning null");
            }
            else
            {
                output = Spawner.Instance.Spawn(_items.List[index].index, position, rotation, scale);
            }

            return output;
        }
    }
}