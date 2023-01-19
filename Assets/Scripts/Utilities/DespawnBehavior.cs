using System.Collections;
using Alteruna;
using UnityEngine;

[RequireComponent(typeof(Owner))]
public class DespawnBehavior : AttributesSync
{
    [SerializeField] protected float _lifetime = 5.0f;
    private Owner _owner;
    private void Start()
    {
        if (Multiplayer == null)
        {
            Debug.LogWarning("My name is Unity, and I'm fucking stupid");
            return;
        }

        _owner = this.GetComponent<Owner>();
        
        bool differentUser = _owner.ID != Multiplayer.Me.Index;

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
        Utilities.Singletons.Spawner.Instance.RequestDespawn(_owner.ID, this.gameObject);
    }
}