using System;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(VFXSpawnController), typeof(Spawner))]
public class PickupBox : MonoBehaviour
{
    private Alteruna.Spawner _spawner;
    private VFXSpawnController _vfx;
    
    [SerializeField] private int _indexForPlayerVFX = 0;
    [SerializeField] private int _indexForDespawnVFX = 0;
    [SerializeField] private float _pickupDuration = 5.0f;
    [SerializeField] private UnityEvent _callback;
    [SerializeField] private string checkTag = "Player";

    private void Start()
    {
        _spawner = this.GetComponent<Alteruna.Spawner>();
        _vfx = this.GetComponent<VFXSpawnController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitByPlayer = other.CompareTag(checkTag);

        if (!hitByPlayer)
        {
            return;
        }

        Transform playerTransform = other.transform;

        _vfx.SpawnVFX(_spawner, _indexForDespawnVFX, this.transform);
        _vfx.AttachVFXToTarget(_spawner, _indexForPlayerVFX, playerTransform, playerTransform);

        _callback.Invoke();
        Destroy(this.gameObject);
    }
}