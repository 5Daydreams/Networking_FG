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
   private Alteruna.Avatar avatar;

   private void Awake()
   {
       
       spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
       avatar = GetComponent<Alteruna.Avatar>();
      
   }

   void Start()
    {
        
    }

  

   void Update()
    {
        if (!avatar.IsMe)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnBullet();
            
        }
    }

    void SpawnBullet()
    {
        spawner.Spawn(indexToSpawn, GunPipe.position + GunPipe.forward, GunPipe.rotation);
       
    }
}
