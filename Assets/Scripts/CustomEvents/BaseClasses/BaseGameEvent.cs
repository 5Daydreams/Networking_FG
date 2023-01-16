using System.Collections.Generic;
using UnityEngine;

namespace submodules.unity_spellbook._Code.CustomEvents.BaseEvent
{
    public class BaseGameEvent<T> : ScriptableObject
    {
        private readonly List<IGameEventListener<T>> eventListeners = new List<IGameEventListener<T>>();
        [SerializeField] protected T inspectorValue;

        public void Raise(T item)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
            {
                eventListeners[i].OnEventRaised(item);
            }
        }

        public void InspectorRaise() => Raise(inspectorValue);

        public void RegisterListener(IGameEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(IGameEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }
    }
}