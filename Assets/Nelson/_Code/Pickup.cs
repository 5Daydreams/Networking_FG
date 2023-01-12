using System;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider), typeof(Spawner))]
public class Pickup : MonoBehaviour
{
    private Alteruna.Spawner spawner;
    private Collider col;

    [SerializeField] private int _applyOnPlayerIndex = 0;
    [SerializeField] private int _onCollectedIndex = 0;
    [SerializeField] private float _pickupDuration = 5.0f;
    [SerializeField] private UnityEvent _callback;
    
    private void Awake()
    {
        spawner = this.GetComponent<Alteruna.Spawner>();
    }

    void Start()
    {
        col = this.GetComponent<Collider>();
    }

    private void SpawnVFXOnPickup()
    {
        spawner.Spawn(
            _onCollectedIndex,
            this.transform.position,
            this.transform.rotation
        );
    }

    private void AttachedVFXToTarget(Transform attachTarget)
    {
        GameObject output =
            spawner.Spawn(
                _applyOnPlayerIndex,
                attachTarget.position,
                attachTarget.rotation
            );

        output.transform.parent = attachTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        SpawnVFXOnPickup();
        AttachedVFXToTarget(other.gameObject.transform);

        _callback.Invoke();
        Destroy(this.gameObject);
    }
}