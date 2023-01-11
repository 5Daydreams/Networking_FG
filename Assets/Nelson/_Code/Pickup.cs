using System;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

[RequireComponent(typeof(Collider))]
public class Pickup : MonoBehaviour
{
    private Collider col;

    [SerializeField] private int _indexToSpawn = 0;
    [SerializeField] private UnityEvent _callback;

    private Alteruna.Spawner spawner;

    private void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Alteruna.Spawner>();
    }

    void Start()
    {
        col = this.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject cachedOther = other.gameObject;

        GameObject output =
            spawner.Spawn(
                _indexToSpawn,
                cachedOther.transform.position,
                cachedOther.transform.rotation
                );

        output.transform.parent = cachedOther.transform;

        _callback.Invoke();
        Destroy(this.gameObject);
    }
}