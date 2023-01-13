using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;

public class PlayerHealth : AttributesSync
{
    [SynchronizableField][SerializeField]private int health = 100;
    [SerializeField] private int damage = 20;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [SerializeField] Camera camera;

    [SerializeField] PlayerKDA playerkda;

    public Alteruna.Avatar avatar;
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
            GetTarget();
    }

    public int GetHealt()
    {
        return health;
    }

    void GetTarget()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayer))
        {
            PlayerHealth playerHit = hit.transform.GetComponentInChildren<PlayerHealth>();

            //Add the player who hit befor to the assits.
            if (playerHit.previousDamageDealer != null && avatar != playerHit.previousDamageDealer)
            {
                Debug.Log("ADD TO LIST");
                playerHit.damageDealers.Add(previousDamageDealer);
            }

            playerHit.previousDamageDealer = avatar;
            Debug.Log(playerHit.previousDamageDealer.GetInstanceID());

            playerHit.TakeDamage(damage);
        }
    }



    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            BroadcastRemoteMethod("Die");
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