
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
        
        spawner.Despawn(this.gameObject);
    }

    private IEnumerator KillAfterSeconds(float time)
    {
        // StartCoroutine(KillAfterSeconds(_lifetime));
        yield return null;
        
        yield return new WaitForSeconds(time);
        
        yield return null;
        // Destroy(this.gameObject);
    }
}