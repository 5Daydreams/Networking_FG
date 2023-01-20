using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Debug/MessageDebug", fileName = "EventDebug")]
public class EventDebug : ScriptableObject
{
    public void MessageLog(string value)
    {
        Debug.Log(value);
    }
}