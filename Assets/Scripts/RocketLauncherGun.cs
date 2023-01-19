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
        if (!avatar.IsMe)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           // Debug.Log("Spawn bullet");
            SpawnBullet();
            Debug.Log("Multiplayer.Me.Index in gun: " + Multiplayer.Me.Index);
        }
    }

    void SpawnBullet()
    {
       GameObject bullet = spawner.Spawn(indexToSpawn, GunPipe.position + GunPipe.forward, GunPipe.rotation);
       bullet.GetComponentInChildren<RocketLauncherBullet>().UserID = Multiplayer.Me.Index;
       Debug.Log("Multiplayer.Me.Index in gun: " + Multiplayer.Me.Index);
       Debug.Log("UserID in bullet in gun: " + bullet.GetComponentInChildren<RocketLauncherBullet>().UserID);
      
    
    }
}
