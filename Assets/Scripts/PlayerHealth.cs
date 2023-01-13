using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerHealth : AttributesSync
{
    [SynchronizableField] public int health = 100;
    [SerializeField] private int damage = 10;
    public Alteruna.Avatar avatar;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int playerSelfLayer;

    [SerializeField] Camera camera;

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
            PlayerHealth playerShoot = hit.transform.GetComponentInChildren<PlayerHealth>();
            playerShoot.TakeDamage(damage);
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
        Debug.Log("Player Died");
    }
}