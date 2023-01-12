namespace submodules.unity_spellbook._Code.CustomEvents.BaseEvent
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}