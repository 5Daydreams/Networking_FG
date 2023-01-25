using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider), typeof(VFXController))]
public class HealthPickup : MonoBehaviour
{
    private VFXController _vfx;

    [SerializeField] private int _indexForBurstVFX = 0;
    [SerializeField] private int _healValue;
    [SerializeField] private string checkTag = "Player";

    private void Start()
    {
        _vfx = this.GetComponent<VFXController>();
    }
    
    private void PlayVFXResponse()
    {
        _vfx.SpawnVFX(Utilities.Singletons.Spawner.Instance, _indexForBurstVFX, this.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitByPlayer = other.CompareTag(checkTag);

        if (!hitByPlayer)
        {
            return;
        }

        PlayVFXResponse();

        other.GetComponentInChildren<PlayerHealth>().AddHealt(_healValue);
        this.gameObject.SetActive(false);

        // Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
    }
}