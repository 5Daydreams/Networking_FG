using System;
using Alteruna;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Alteruna.Spawner))]
public class BurstVFX : MonoBehaviour
{
    private Spawner _spawner;
    [SerializeField] private int _indexForVFX;

    private void Start()
    {
        _spawner = this.GetComponent<Alteruna.Spawner>();
    }

    public void Spawn()
    {
        _spawner.Spawn(_indexForVFX, transform.position, transform.rotation);
    }
}