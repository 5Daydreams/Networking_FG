using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RocketLauncherGun : AttributesSync
{ 
   [SerializeField] private int indexToSpawn = 0;
   [SerializeField] Transform GunPipe;
   [SerializeField]
   public Camera camera;

   [SerializeField] private float maxRayLenght = 1000f;
   [SerializeField] private float bulletMaxLength = 100f;

   private Ray ray;
   

   private Vector3 HitPoint;
   
   
   private Spawner spawner;
   public Alteruna.Avatar avatar;
   

   private void Awake()
   {
       spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
       //camera = GetComponentInParent<Camera>();
   }

   void Start()
    {
        //avatar = transform.GetComponent<Alteruna.Avatar>();
    }
   void Update()
    {
        // Hi Johanna - I'm sorry, but I need to debug this
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
       RaycastHit Hit;
       ray = new Ray(camera.transform.position, camera.transform.forward);
       
       if (Physics.Raycast(ray, out Hit, maxRayLenght))
       {
           //Debug.DrawRay(camera.transform.position, camera.transform.forward *100f, Color.red,1000f);
           HitPoint = Hit.point;
       }
       else
       {
           HitPoint = ray.GetPoint(bulletMaxLength);
       }
       GameObject bullet = spawner.Spawn(indexToSpawn, GunPipe.position + GunPipe.forward, GunPipe.rotation);
       bullet.GetComponentInChildren<RocketLauncherBullet>().UserID = Multiplayer.Me.Index;
       bullet.GetComponentInChildren<RocketLauncherBullet>().direction = HitPoint- GunPipe.position;
   }
}
