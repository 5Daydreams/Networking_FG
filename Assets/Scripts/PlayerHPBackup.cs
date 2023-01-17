using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerHpBackup : AttributesSync
{
   /* [Header("Health")]
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

    [SerializeField] PlayerKDA playerkda;

    [SerializeField] Camera camera;

    public Alteruna.Avatar localAvatar;

    [SynchronizableField] public int previousDamageDealer;
    [SynchronizableField] public int assistBitmask;

    AvatarCollection avatarCollection;

    private void Start()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();
        baseHealth = health;
        baseAssistTimer = assistTimer;
        if (localAvatar.IsMe)
        {
            previousDamageDealer = localAvatar.Possessor.Index;
            localAvatar.gameObject.layer = playerSelfLayer;
        }
    }

    private void Update()
    {
        if (!localAvatar.IsMe)
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
            DealDamage(20, playerHp);
        }
    }

    public void DealDamage(int damageTaken, PlayerHealth playerHit)
    {
        playerHit.previousDamageDealer = localAvatar.Possessor.Index;

        int assists = 1 << localAvatar.Possessor.Index;
        playerHit.assistBitmask |= assists;

        playerHit.health -= damageTaken;

        if (playerHit.health <= 0)
        {
            playerHit.Die(localAvatar.Possessor.Index);
        }
    }

    void Die(int killer)
    {
        playerkda.AddDeath(1);

        if (previousDamageDealer != localAvatar.Possessor.Index)
            avatarCollection.avatars[previousDamageDealer].GetComponentInChildren<PlayerKDA>().AddKill(1);

        if (avatarCollection.avatars[assistBitmask] == 1)
        {
            foreach (var item in collection)
            {
                if ()
                {
                    avatarCollection.avatars[assistBitmask[i]].GetComponentInChildren<PlayerKDA>().AddAssist(1);
                    assistBitmask.RemoveAt(i);
                }
            }
            // Multiplayer.GetAvatar((ushort)i).GetComponentInChildren<PlayerKDA>().AddAssist(1);
        }
        //UPDATEKDATEXT
        Spawn();
    }


    void Spawn()
    {
        //ClearDamageDealers();
        //Spawn timer
        health = baseHealth;
        //Respawn
    }

    void ClearDamageDealers()
    {
        previousDamageDealer = localAvatar.Possessor.Index;
        assistBitmask.Clear();
    }

    void AssistTimer()
    {
        assistTimer = baseAssistTimer;
        assistTimer -= Time.deltaTime;

        //remove assisting player from the bottom up
        for (int i = assistBitmask.Count - 1; i >= 0; i--)
        {
            if (assistTimer <= 0)
            {
                previousDamageDealer = localAvatar.Possessor.Index;
                assistBitmask.RemoveAt(i);
                //remove assisting player from the bottom up
            }
        }
    }*/
}