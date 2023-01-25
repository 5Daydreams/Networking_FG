using System;
using Alteruna;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class PickupHolder : AttributesSync
{
    [SerializeField] private GameObject _pickup;
    [SerializeField] private VisualEffect _spawnVFX;
    [Header("Timer")] 
    [SerializeField] private float _minDelay = 7.0f;
    [SerializeField] private float _maxDelay = 15.0f;

    private float _currTimer = 0.0f;

    private bool _pickupAvailable
    {
        get => _pickup.activeInHierarchy;

        set => _pickup.SetActive(value);
    }

    public void StartLogic()
    {
#if UNITY_EDITOR
        if (_pickup == null)
        {
            _pickup = GetComponentInChildren<HealthPickup>().gameObject;

            if (_pickup == null)
            {
                Debug.LogError("No pickup set via inspector or found as a child Object " +
                               "- please double check hierarchy setup");
                UnityEditor.EditorApplication.isPaused = true;
            }
        }
#endif
        _pickupAvailable = false;
        ResetTimer();
    }

    private void ResetTimer()
    {
        // This check is necessary to avoid multiple users trying to reset this
        if (_pickupAvailable)
        {
            return;
        }
        
        _currTimer = Random.Range(_minDelay, _maxDelay);
    }

    private void Update()
    {
        if (_pickupAvailable)
        {
            return;
        }
        
        _currTimer -= Time.deltaTime;
        
        if (_currTimer <= 0.0f)
        {
            ResetTimer();
            _pickupAvailable = true;
            _spawnVFX.SendEvent("OnPlay");
        }
    }
}