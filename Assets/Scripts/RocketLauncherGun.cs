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
   [SerializeField] public Camera camera;

   [SerializeField] private float maxRayLenght = 1000f;
   [SerializeField] private float maxBulletLength = 100f;
   [SerializeField] private float shootTimer = 3f;
   private float defaultShootTimer = 3f;
   private bool IsRealoading = false;
   
   private Ray ray;
   private Vector3 HitPoint;
   private Spawner spawner;
   public Alteruna.Avatar avatar;
   
   [SerializeField] private LayerMask playerLayer;
   [SerializeField] PlayerHealth playerHealth;


    private void Awake()
    {
       spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
       defaultShootTimer = shootTimer;
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
        if (Input.GetKey(KeyCode.Mouse0) && !IsRealoading && !playerHealth.dead)
        {
           // DirectHit();
            SpawnBullet();
            IsRealoading = true;
        }
        if (IsRealoading)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                IsRealoading = false;
                shootTimer = defaultShootTimer;
            }
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
           HitPoint = ray.GetPoint(maxBulletLength);
       }
       GameObject bullet = spawner.Spawn(indexToSpawn, GunPipe.position + GunPipe.forward, GunPipe.rotation);
       bullet.GetComponentInChildren<RocketLauncherBullet>().UserID = Multiplayer.Me.Index;
       bullet.GetComponentInChildren<RocketLauncherBullet>().direction = HitPoint- GunPipe.position;
   }

   void DirectHit()
   {
      if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, Mathf.Infinity, playerLayer))
      {
          if (hit.transform.gameObject.layer == playerLayer)
          {
              Debug.Log("DirectHit in gun with raycast");
          }
      }
   }
}
