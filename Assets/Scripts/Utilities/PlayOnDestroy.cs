using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayOnDestroy : MonoBehaviour
{
    [SerializeField] private UnityEvent _callback;
    private void OnDestroy()
    {
        _callback.Invoke();
    }
}