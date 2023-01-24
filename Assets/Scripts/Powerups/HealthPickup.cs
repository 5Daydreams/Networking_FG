using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider),  typeof(VFXController))]
public class HealthPickup : MonoBehaviour
{
    private VFXController _vfx;

    // [SerializeField] private int _indexForPlayerVFX = 0;
    [SerializeField] private int _indexForBurstVFX = 0;

    [SerializeField] private int _healValue;
    [SerializeField] private string checkTag = "Player";

    private void Start()
    {
        _vfx = this.GetComponent<VFXController>();
    }

    private void PlayVFXResponse(Transform playerTransform)
    {
        _vfx.SpawnVFX(Utilities.Singletons.Spawner.Instance, _indexForBurstVFX, this.transform);
        // // Commenting this out because the health isn't going to stick to the player like the buffs would
        // _vfx.AttachVFXToTarget(Utilities.Singletons.Spawner.Instance, _indexForPlayerVFX, playerTransform,
        //     playerTransform);        
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitByPlayer = other.CompareTag(checkTag);

        if (!hitByPlayer)
        {
            return;
        }
        
        PlayVFXResponse(other.transform);

        other.GetComponentInChildren<PlayerHealth>().AddHealt(_healValue);
        Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
    }
}