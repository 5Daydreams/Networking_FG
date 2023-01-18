using System;
using System.Collections;
using Alteruna;
using UnityEngine;

public class DespawnBehavior : AttributesSync
{
    [SerializeField] protected float _lifetime = 5.0f;
    private int _ownerID = -1;
    
    public void SetOwner(int id)
    {
        _ownerID = id;
    }

    private void OnEnable()
    {
        bool differentUser = (int) _ownerID != (int) Multiplayer.Me.Index;
        
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