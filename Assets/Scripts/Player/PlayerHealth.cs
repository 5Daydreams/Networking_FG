using UnityEngine;
using Alteruna;

public class PlayerHealth : AttributesSync
{
    [SynchronizableField] public int health = 100;
    [SerializeField] private int damage = 20;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [SerializeField] Camera camera;

    [SerializeField] PlayerKDA playerkda;

    public Alteruna.Avatar avatar;
    [HideInInspector]
    public Alteruna.Avatar lastAvatarHit;

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

    void GetTarget()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayer))
        {
            PlayerHealth playerHit = hit.transform.GetComponentInChildren<PlayerHealth>();
            playerHit.TakeDamage(damage);
            playerHit.lastAvatarHit = avatar;
        }
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        if (health <= 0)
        {
            BroadcastRemoteMethod("Die");
            playerkda.AddDeath(1);
        }
    }

    [SynchronizableMethod]
    void Die()
    {
        Debug.Log("Player Died");
        lastAvatarHit.GetComponentInChildren<PlayerKDA>().AddKill(1);
    }
}