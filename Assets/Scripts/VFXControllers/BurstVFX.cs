using System;
using Alteruna;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VFXController), typeof(Spawner))]
public class BurstVFX : MonoBehaviour
{
    private VFXController _vfxSpawner;
    [SerializeField] private int _indexForVFX;

    private void Start()
    {
        _vfxSpawner = this.GetComponent<VFXController>();
    }

    public void Spawn()
    {
        _vfxSpawner.SpawnVFX(Utilities.Spawner.Instance, _indexForVFX, this.transform);
    }
}