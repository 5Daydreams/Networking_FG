using System;
using System.Collections;
using Alteruna;
using UnityEngine;

public class DespawnBehavior : MonoBehaviour
{
    [SerializeField] protected float _lifetime = 5.0f;

    private void OnEnable()
    {
        StartCoroutine(KillAfterSeconds(_lifetime));
    }

    private IEnumerator KillAfterSeconds(float time)
    {
        yield return null;

        yield return new WaitForSeconds(time);
        Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
    }
}