using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class RocketLauncherGun : MonoBehaviour
{ 
    [SerializeField] private int indexToSpawn = 0;
    [SerializeField] Transform GunPipe;
   
   private Spawner spawner;
   public Alteruna.Avatar avatar;

   private void Awake()
   {
       
       spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
       //avatar = GetComponent<Alteruna.Avatar>();
       //avatar = gameObject.GetComponent<Alteruna.Avatar>();
      


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
            Debug.Log("Spawn bullet");
            SpawnBullet();
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DeSpawnBullet();
        }
    }

    void SpawnBullet()
    {
        spawner.Spawn(indexToSpawn, GunPipe.position + GunPipe.forward, GunPipe.rotation);
        
      //  spawner.Despawn();
       
    }

    void DeSpawnBullet()
    {
       var spawnedObjects = spawner.SpawnedObjects;
       foreach (var bullet in spawnedObjects)
       {
           spawner.Despawn(bullet.Item1);
           
           Destroy(bullet.Item1);
       }
       
    }
}
