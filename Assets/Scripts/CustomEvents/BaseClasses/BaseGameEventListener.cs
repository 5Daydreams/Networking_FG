using System;
using UnityEngine;
using UnityEngine.Events;

namespace submodules.unity_spellbook._Code.CustomEvents.BaseEvent
{
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour,
        IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField] private E _gameEvent;
        public E GameEvent => _gameEvent;
        [SerializeField] private UER UnityEventResponse;

        private void OnEnable()
        {
            if (_gameEvent == null)
                return;

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (_gameEvent == null)
                return;
            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            UnityEventResponse?.Invoke(item);
        }
    }

    [Serializable]
    public struct EventAndResponse<T, E, UER> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        public E gameEvent;
        public UER UnityEventResponse;
    }
}