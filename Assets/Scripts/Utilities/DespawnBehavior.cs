using System;
using System.Collections;
using Alteruna;
using UnityEngine;

[RequireComponent(typeof(Owner))]
public class DespawnBehavior : AttributesSync
{
    [SerializeField] protected bool _useNetwork = false;
    [SerializeField] protected bool _startByDefault = true;
    [SerializeField] protected float _lifetime = 5.0f;
    private Owner _owner;

    private void Start()
    {
        if (_startByDefault)
        {
            StartCountdown();
        }
    }

    public void StartCountdown()
    {
        if (_useNetwork)
        {
            if (Multiplayer.Me == null)
            {
                Debug.LogWarning("Multiplayer Room not entered yet, user not initialized");
                return;
            }

            _owner = this.GetComponent<Owner>();

            if (_owner == null)
            {
                Debug.LogError("I have no idea what happened. Somehow this spawned without an owner?");
                return;
            }

            Alteruna.Multiplayer multiplayer = GameObject.FindWithTag("NetworkManager").GetComponent<Multiplayer>();
            bool differentUser = _owner.ID != multiplayer.Me.Index;

            if (differentUser)
            {
                return;
            }
        }

        StartCoroutine(KillAfterSeconds(_lifetime));
    }

    private IEnumerator KillAfterSeconds(float time)
    {
        yield return null;

        yield return new WaitForSeconds(time);

        if (_useNetwork)
        {
            Utilities.Singletons.Spawner.Instance.RequestDespawn(_owner.ID, this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}