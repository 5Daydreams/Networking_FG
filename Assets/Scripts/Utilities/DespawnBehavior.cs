using System;
using System.Collections;
using Alteruna;
using UnityEngine;

public class DespawnBehavior : AttributesSync
{
    [SerializeField] protected float _lifetime = 5.0f;

    [HideInInspector] [SynchronizableField]
    public int OwnerID = -1;

    private void OnEnable()
    {
        bool differentUser = (int) OwnerID != (int) Multiplayer.Me.Index;

        if (differentUser)
        {
            return;
        }

        StartCoroutine(KillAfterSeconds(_lifetime));
    }

    private IEnumerator KillAfterSeconds(float time)
    {
        yield return null;

        yield return new WaitForSeconds(time);
        Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
    }
}