using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace CustomScriptables
{
    [CreateAssetMenu(menuName = "Scriptables/SpawnerList", fileName = "SpawnerList")]
    public class SpawnerList : ScriptableObject
    {
        public List<SpawnerEntry> List;
        
        public int FindIndexInList(string key)
        {
            for (int i = 0; i < List.Count; i++)
            {
                bool nameFound = List[i].key == key;
                if (nameFound)
                {
                    return List[i].index;
                }
            }

            return -1;
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