using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class VFXController : MonoBehaviour
{
    private VisualEffect _effect;
    [SerializeField] private string _spawnRateString = "SpawnRate";

    private void Awake()
    {
        _effect = this.GetComponent<VisualEffect>();
    }

    public void InvokeVFXEvent(string eventName)
    {
        _effect.SendEvent(eventName);
    }

    // Note - this requires setting up a variable within the VFX Graph with the corresponding name
    public void SetSpawnRate(float value)
    {
        _effect.SetFloat(_spawnRateString, value);
    }
    
    public void SpawnVFX(Alteruna.Spawner spawner, int spawnerIndex, Transform targetTransform)
    {
        spawner.Spawn(
            spawnerIndex,
            targetTransform.position,
            targetTransform.rotation
        );
    }

    public void AttachVFXToTarget(Alteruna.Spawner spawner, int spawnerIndex,
        Transform spawnTransform, Transform attachTransform)
    {
        GameObject output =
            spawner.Spawn(
                spawnerIndex,
                spawnTransform.position,
                spawnTransform.rotation
            );

        output.transform.parent = attachTransform;
    }
}