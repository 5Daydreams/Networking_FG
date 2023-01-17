using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;
using Unity.Burst.CompilerServices;

public class PlayerHPBackup : AttributesSync
{
    /*
    [SerializeField] private int damage = 20;// remove when in weapons script

    [Header("Health")]
    [SynchronizableField][SerializeField] private int health = 100;
    private int baseHealth;

    [Header("Spawn")]
    public int spawnTimer;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [Header("KDA")]
    [SerializeField] private float assistTimer;
    private float baseAssistTimer;

    [SerializeField] LeaderBoardManager leaderBoardManager;

    [SerializeField] Camera camera;

    public Alteruna.Avatar avatar;

    [SynchronizableField]
    public int previousDamageDealer;
    [SynchronizableField]
    public List<int> assistingPlayers = new List<int>();

    //public List<float> assisstTimers = new List<float>();



    private void Start()
    {
        if (avatar.IsMe)
        {
            avatar.gameObject.layer = playerSelfLayer;
            baseHealth = health;
            baseAssistTimer = assistTimer;
            previousDamageDealer = avatar.Possessor.Index;
        }
        else
            return;
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
            //BroadcastRemoteMethod("TakeDamage", damage, playerHp);
        }
    }

    public void TakeDamage(int damageTaken, PlayerHealth playerHitHp)
    {
        if (avatar.Possessor.Index != playerHitHp.previousDamageDealer)
        {
            Debug.Log("ADD TO LIST");
            playerHitHp.assistingPlayers.Add(playerHitHp.previousDamageDealer);
            //assisstTimers.Add(assistTimer);
        }

        playerHitHp.previousDamageDealer = avatar.Possessor.Index;
        Debug.Log("ID = " + playerHitHp.previousDamageDealer);

        playerHitHp.health -= damageTaken;
        //UPDATE HEALT TEXT

        if (playerHitHp.health <= 0)
        {
            avatar.GetComponent<PlayerKDA>().kills += 1;
            playerHitHp.GetComponent<PlayerKDA>().deaths += 1;
            //leaderBoardManager.AddDeath(playerHitHp.avatar.Possessor.Index);
        }
    }

    void GetKill()
    {
        leaderBoardManager.AddKill(previousDamageDealer);

        for (int i = assistingPlayers.Count - 1; i > 0; i--)
        {
            //if (assisstTimers[i] > 0)
            //assistingPlayers[i].GetComponentInChildren<PlayerKDA>().AddAssist(1);
            assistingPlayers.RemoveAt(i);

        }
        //UPDATEKDATEXT
        Spawn();
    }


    void Spawn()
    {
        ClearDamageDealers();
        //Spawn timer
        if (avatar.IsMe)
            health = baseHealth;
        //Respawn
    }

    void ClearDamageDealers()
    {
        previousDamageDealer = avatar.Possessor.Index;
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
                //previousDamageDealer = null;
                assistingPlayers.RemoveAt(i);
                //remove assisting player from the bottom up
            }
        }
    }*/
}