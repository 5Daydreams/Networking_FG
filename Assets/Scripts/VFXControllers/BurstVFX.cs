using System;
using Alteruna;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VFXSpawnController), typeof(Spawner))]
public class BurstVFX : MonoBehaviour
{
    private Spawner _alterunaSpawner;
    private VFXSpawnController _vfxSpawner;
    [SerializeField] private int _indexForVFX;

    private void Start()
    {
        _alterunaSpawner = this.GetComponent<Alteruna.Spawner>();
        _vfxSpawner = this.GetComponent<VFXSpawnController>();
    }

    public void Spawn()
    {
        _vfxSpawner.SpawnVFX(_alterunaSpawner, _indexForVFX, this.transform);
    }
}