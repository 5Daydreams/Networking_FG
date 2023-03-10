using UnityEngine;
using Alteruna;
using System.Collections.Generic;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerHealth : AttributesSync
{
    [Header("Health")]
    [SynchronizableField][SerializeField]private int health = 100;
    private int baseHealth;

    [Header("Spawn")]
    public float spawnTimer = 3;
    [HideInInspector]public float baseSpwanTime;
    public RigidbodySynchronizable rb;
    public Rigidbody rbUnity;
    public CapsuleCollider collider;
    public MeshRenderer[] disableMeshOnDeath;
    [SynchronizableField] public bool dead = false;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [Header("KDA")]
    [SerializeField]private float assistTimer;
    [SerializeField] PlayerKDA playerkda;
    private float baseAssistTimer;

    [SerializeField] Camera camera;

    public Alteruna.Avatar localAvatar;

    [Header("Damage Dealers")]
    [SynchronizableField] public int previousDamageDealer;
    [SynchronizableField] public List<int> assistingPlayers = new List<int>();

    [Header("Team Manager")]
    [SerializeField] private TeamManagerSync teamManagerSync;

    AvatarCollection avatarCollection;
    Leaderboard leaderboard;
    PlayerRespawn playerRespawn;
    GameModeManager gameModeManager;
    private void Awake()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();
        leaderboard = FindObjectOfType<Leaderboard>();
        playerRespawn = FindObjectOfType<PlayerRespawn>();
        gameModeManager = FindObjectOfType<GameModeManager>();
        baseHealth = health;
        baseSpwanTime = spawnTimer;
    }

    private void Start()
    {
        if (localAvatar.IsMe)
        {
            baseAssistTimer = assistTimer;
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

    public int GetHealth()
    {
        return health;
    }

    public int AddHealt(int amount)
    {
        return health += amount;
    }

    //will be done in the weapon script later;
    void Shoot()
    {
       // if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayer))
       // {
       //     PlayerHealth playerHp = hit.transform.GetComponentInChildren<PlayerHealth>();
       //     DealDamage(20, playerHp);
       // }
    }

    public void DealDamage(int damageTaken, PlayerHealth playerHit)
    {
        playerHit.previousDamageDealer = localAvatar.Possessor.Index;
        playerHit.assistingPlayers.Add(localAvatar.Possessor.Index);

        playerHit.health -= damageTaken;

        if (playerHit.health <= 0)
        {
            gameModeManager.UpdateTeamKills(teamManagerSync.teamID);
            playerHit.Die(localAvatar.Possessor.Index);
        }
    }

    void Die(int killer)
    {
        playerkda.AddDeath(1);

        if (previousDamageDealer != localAvatar.Possessor.Index)
            avatarCollection.avatars[previousDamageDealer].GetComponentInChildren<PlayerKDA>().AddKill(1);
        else // - kills if you kill your self
            avatarCollection.avatars[previousDamageDealer].GetComponentInChildren<PlayerKDA>().AddKill(-1);

        if (assistingPlayers.Count < 0)
        {
            for (int i = 0; i < assistingPlayers.Count; i++)
            {
                if (assistingPlayers[i] != killer)
                {
                    avatarCollection.avatars[assistingPlayers[i]].GetComponentInChildren<PlayerKDA>().AddAssist(1);
                    assistingPlayers.RemoveAt(i);
                }
            }
                // Multiplayer.GetAvatar((ushort)i).GetComponentInChildren<PlayerKDA>().AddAssist(1);
        }
        //UPDATEKDATEXT
        leaderboard.BroadcastRemoteMethod("UpdateScoreboard");
        BroadcastRemoteMethod("BrodcastCoroutine");
        //Brodcas
        //StartCoroutine(Spawn());
    }

    [SynchronizableMethod]
    void BrodcastCoroutine()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        dead = true;
        collider.enabled = false;

        for (int i = 0; i < disableMeshOnDeath.Length; i++)
        {
            disableMeshOnDeath[i].enabled = false;
        }

        ClearDamageDealers();

        spawnTimer = baseSpwanTime;
        yield return new WaitForSeconds(spawnTimer);

        //Set spwan position
        playerRespawn.Respawn(rb);

        health = baseHealth;
        collider.enabled = true;
        dead = false;

        for (int i = 0; i < disableMeshOnDeath.Length; i++)
        {
            disableMeshOnDeath[i].enabled = true;
        }
    }

    void ClearDamageDealers()
    {
        previousDamageDealer = localAvatar.Possessor.Index;
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
                previousDamageDealer = localAvatar.Possessor.Index;
                assistingPlayers.RemoveAt(i);
                //remove assisting player from the bottom up
            }
        }
    }
}