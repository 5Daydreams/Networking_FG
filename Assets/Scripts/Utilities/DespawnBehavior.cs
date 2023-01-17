
using System;
using System.Collections;
using Alteruna;
using UnityEngine;

public class DespawnBehavior : MonoBehaviour
{
    [SerializeField] protected float _lifetime = 5.0f;
    private Spawner spawner;

    private void OnEnable()
    {
        spawner = GameObject.FindWithTag("NetworkManager").GetComponent<Spawner>();
        
        StartCoroutine(KillAfterSeconds(_lifetime));
    }

    private IEnumerator KillAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        spawner.Despawn(this.gameObject);
        yield return null;
        Destroy(this.gameObject);
    }
}