using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class RocketLauncherGun : AttributesSync
{ 
    [SerializeField] private int indexToSpawn = 0;
    [SerializeField] Transform GunPipe;
   
   private Spawner spawner;
   public Alteruna.Avatar avatar;

   private void Awake()
   {
       spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
   }

   void Start()
    {
        //avatar = transform.GetComponent<Alteruna.Avatar>();
    }
   void Update()
    {
        // if (!avatar.IsMe)
        // {
        //     return;
        // }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Spawn bullet");
            SpawnBullet();
        }
    }

    void SpawnBullet()
    {
       GameObject bullet = spawner.Spawn(indexToSpawn, GunPipe.position + GunPipe.forward, GunPipe.rotation);
       bullet.GetComponent<RocketLauncherBullet>().UserID = Multiplayer.Me.Index;
       Debug.Log("Spawn bullet me index: " +Multiplayer.Me.Index);
    }
}
