using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;
using Unity.Burst.CompilerServices;

public class PlayerHealth : AttributesSync
{
    [SynchronizableField][SerializeField]private int health = 100;
    [SerializeField] private int damage = 20;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [SerializeField] Camera camera;

    [SerializeField] PlayerKDA playerkda;

    public Alteruna.Avatar avatar;
    //[SynchronizableField]
    public Alteruna.Avatar previousDamageDealer;

    public List<Alteruna.Avatar> damageDealers = new List<Alteruna.Avatar>();

    private void Start()
    {
        if (avatar.IsMe)
            avatar.gameObject.layer = playerSelfLayer;
    }

    private void Update()
    {
        if (!avatar.IsMe)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
            Shoot();
    }

    public int GetHealt()
    {
        return health;
    }

    //will be done in the weapon script later;
    void Shoot()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayer))
        {
            PlayerHealth playerHp = hit.transform.GetComponentInChildren<PlayerHealth>();
            TakeDamage(damage, playerHp);
        }
    }

    public void TakeDamage(int damageTaken, PlayerHealth playerHitHp)
    {
        if (playerHitHp.previousDamageDealer != null && avatar != playerHitHp.previousDamageDealer)
        {
            Debug.Log("ADD TO LIST");
            playerHitHp.damageDealers.Add(playerHitHp.previousDamageDealer);
        }

        playerHitHp.previousDamageDealer = avatar;
        Debug.Log(playerHitHp.previousDamageDealer.GetInstanceID());

        playerHitHp.health -= damageTaken;

        if (playerHitHp.health <= 0)
        {
            playerHitHp.BroadcastRemoteMethod("Die");
        }
    }

    [SynchronizableMethod]
    void Die()
    {
        playerkda.AddDeath(1);

        // if no dmg dealer, give no kills
        if (previousDamageDealer != null)
            previousDamageDealer.GetComponentInChildren<PlayerKDA>().AddKill(1);

        for (int i = damageDealers.Count - 1; i > 0; i--)
        {
            damageDealers[i].GetComponentInChildren<PlayerKDA>().AddAssist(1);
            damageDealers.RemoveAt(i);
        }

        previousDamageDealer = null;
    }

    [SynchronizableMethod]
    void Spawn()
    {

    }
}