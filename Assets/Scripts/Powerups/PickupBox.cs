using System;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(VFXController))]
public class PickupBox : MonoBehaviour
{
    private VFXController _vfx;
    
    [SerializeField] private int _indexForPlayerVFX = 0;
    [SerializeField] private int _indexForDespawnVFX = 0;
    [SerializeField] private float _pickupDuration = 5.0f;
    [SerializeField] private UnityEvent _callback;
    [SerializeField] private string checkTag = "Player";

    private void Start()
    {
        _vfx = this.GetComponent<VFXController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitByPlayer = other.CompareTag(checkTag);

        if (!hitByPlayer)
        {
            return;
        }

        Transform playerTransform = other.transform;

        _vfx.SpawnVFX(Utilities.Singletons.Spawner.Instance, _indexForDespawnVFX, this.transform);
        _vfx.AttachVFXToTarget(Utilities.Singletons.Spawner.Instance, _indexForPlayerVFX, playerTransform, playerTransform);

        _callback.Invoke();
        Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
    }
}