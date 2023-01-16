using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
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