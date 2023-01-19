using UnityEngine;
using UnityEngine.Events;

public class CallOnDestroy : MonoBehaviour
{
    [SerializeField] private UnityEvent _callback;
    
    private void OnDestroy()
    {
        _callback.Invoke();
    }
}