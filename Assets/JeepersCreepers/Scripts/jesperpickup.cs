using System;
using Alteruna;
using UnityEngine;

public class jesperpickup : MonoBehaviour
{
    private Alteruna.Spawner _spawner;
    private VFXController _vfx;

    [SerializeField] private int _indexForPlayerVFX = 0;
    [SerializeField] private int _indexForDespawnVFX = 0;
    [SerializeField] private float _pickupDuration = 5.0f;
    [SerializeField] private string checkTag = "Player";
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private PickupEffect pickupEffect;


    public enum Effect // your custom enumeration
    {
        heal,
        movementSpeedUp
    };

    [SerializeField] public Effect currentEffect = Effect.heal;


    private void Start()
    {
        _spawner = this.GetComponent<Alteruna.Spawner>();
        _vfx = this.GetComponent<VFXController>();

        SetMaterialColor(currentEffect);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitByPlayer = other.CompareTag(checkTag);

        if (!hitByPlayer)
        {
            return;
        }

        Transform playerTransform = other.transform;

        ActivateEffect(currentEffect, other);

        _vfx.SpawnVFX(Utilities.Singletons.Spawner.Instance, _indexForDespawnVFX, this.transform);
        _vfx.AttachVFXToTarget(Utilities.Singletons.Spawner.Instance, _indexForPlayerVFX, playerTransform, playerTransform);

        Destroy(this.gameObject);
    }

    private void ActivateEffect(Effect effect, Collider player)
    {
        switch (effect)
        {
            // Add health to the player
            case Effect.heal:
                player.GetComponentInChildren<PlayerHealth>().AddHealt(pickupEffect.HealthUp());
                Debug.Log("Heal");
                break;

            // Add movement speed to the player
            case Effect.movementSpeedUp:
                // coroutine? get buff -> wait x seconds -> reset movementspeed
                //player.GetComponent<PlayerMovement>().SetMovementSpeed() += pickupEffect.MovementSpeedUp();
                Debug.Log("Movement speed up");
                break;
        }
    }

    private void SetMaterialColor(Effect effect)
    {
        // Visuals update depending on Effect
        switch (effect)
        {
            
            case Effect.heal:
                renderer.material.color = Color.green;
                break;
            
            case Effect.movementSpeedUp:
                renderer.material.color = Color.red;
                break;
        }
    }
}
