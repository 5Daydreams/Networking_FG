using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    private Collider col;
    [SerializeField] private UnityEvent _callback;
    [SerializeField] private VisualEffectAsset _vfxAsset;

    void Start()
    {
        col = this.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject cachedOther = other.gameObject;
        Instantiate(_vfxAsset, cachedOther.transform);

        _callback.Invoke();
        Destroy(this.gameObject);
    }
}