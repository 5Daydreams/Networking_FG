using UnityEngine;
using UnityEngine.VFX;
using Utilities.Singletons;

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
    
    public void SpawnVFX(ExtendedSpawner spawner, int spawnerIndex, Transform targetTransform)
    {
        spawner.SpawnByIndex(
            spawnerIndex,
            targetTransform.position,
            targetTransform.rotation
        );
    }

    public void AttachVFXToTarget(ExtendedSpawner spawner, int spawnerIndex,
        Transform spawnTransform, Transform attachTransform)
    {
        GameObject output =
            spawner.SpawnByIndex(
                spawnerIndex,
                spawnTransform.position,
                spawnTransform.rotation
            );

        output.transform.parent = attachTransform;
    }
}