using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;
using Unity.Burst.CompilerServices;

public class PlayerHealth : AttributesSync
{
    [SerializeField] private int damage = 20;// remove when in weapons script

    [Header("Health")]
    [SynchronizableField][SerializeField]private int health = 100;
    private int baseHealth;

    [Header("Spawn")]
    public int spawnTimer;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [Header("KDA")]
    [SerializeField]private float assistTimer;
    private float baseAssistTimer;

    [SerializeField] PlayerKDA playerkda;

    [SerializeField] Camera camera;

    public Alteruna.Avatar avatar;

    //[SynchronizableField]
    public GameObject previousDamageDealer;
    //[SynchronizableField]
    public List<GameObject> assistingPlayers = new List<GameObject>();

    //public List<float> assisstTimers = new List<float>();

    private void Start()
    {
        if (avatar.IsMe)
            avatar.gameObject.layer = playerSelfLayer;

        baseHealth = health;
        baseAssistTimer = assistTimer;
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
            playerHitHp.assistingPlayers.Add(playerHitHp.previousDamageDealer.gameObject);
            //assisstTimers.Add(assistTimer);
        }

        playerHitHp.previousDamageDealer = avatar.gameObject;
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

        for (int i = assistingPlayers.Count - 1; i > 0; i--)
        {
            assistingPlayers[i].GetComponentInChildren<PlayerKDA>().AddAssist(1);
            assistingPlayers.RemoveAt(i);
        }

        Spawn();
    }

    [SynchronizableMethod]
    void Spawn()
    {
        ClearDamageDealers();
        //Spawn timer
        health = baseHealth;
        //Respawn
    }

    void ClearDamageDealers()
    {
        previousDamageDealer = null;
        assistingPlayers.Clear();
    }

    void AssistTimer()
    {
        assistTimer = baseAssistTimer;
        assistTimer -= Time.deltaTime;

        //remove assisting player from the bottom up
        for (int i = assistingPlayers.Count - 1; i >= 0; i--)
        {
            if (assistTimer <= 0)
            {
                previousDamageDealer = null;
                assistingPlayers.RemoveAt(i);
                //remove assisting player from the bottom up
            }
        }
    }
}