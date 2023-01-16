using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class RocketLauncherBullet : AttributesSync
{
    private TransformSynchronizable _transform;
    private Vector3 startPosition;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletMaxLength = 50f;

   // public float hitPoints = 100.0F;
    public SphereCollider coll;

    private LayerMask playerLayer;
    private Collider[] hitColliders;
    private void Awake()
    {
        _transform = GetComponent<Alteruna.TransformSynchronizable>();
        startPosition = _transform.transform.position;
    }

    void Start()
    {
       coll = GetComponent<SphereCollider>();
    }
    
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        _transform.transform.Translate(0, 0, bulletSpeed * Time.deltaTime);
        
        if (Vector3.Distance(startPosition, this._transform.transform.position) > bulletMaxLength)
            Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("bullet trigger");
        
        
       
      //  foreach (Collider hitcol in hitColliders)
      //  {
      //      if (hitcol.gameObject.layer == 7)
      //      {
      //          Debug.Log("Hit playerlayer");
      //          var avatar = hitcol.gameObject.GetComponent<Alteruna.Avatar>();
      //          avatar.gameObject.GetComponent<RocketLaunchExplosion>()
      //              .DoExplosion(avatar.gameObject.transform.position, 10f);
      //      }
//
      //      Debug.Log("inside foreach loop");
      //  }

        DoExplosion();
        Destroy(this.gameObject);
    }
    void DoExplosion()
    {
        hitColliders =  Physics.OverlapSphere(this._transform.transform.position, 10f);
        foreach (var hitcol in hitColliders)
        {
            if (hitcol.gameObject.layer == 7) // this works
            {
                Debug.Log(hitcol.gameObject.layer );
                Debug.Log("hitcol.gameObject.layer");
            
                //hitcol.gameObject.GetComponent<RocketLaunchExplosion>().AddExplosionForce();
               //hitcol.GetComponent<RocketLaunchExplosion>().AddExplosionForce(); // this does not work, nullrefference
            }
        }
         
         // if (hitcol.gameObject.CompareTag("Player"))
         // {
         //     Debug.Log("bullet trigger gameobject Player");
         //     hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(gameObject.transform.position, 10f);
         // }
         // if (hitcol.CompareTag("Player"))
         // {
         //     Debug.Log("bullet trigger Player");
         //     hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(gameObject.transform.position, 10f);
         // }

         //if (playerLayer == 7)
         //{
         //    Debug.Log(playerLayer);
         //    Debug.Log("hitcol.gameObject.layer");
         //   
         //    hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(hitcol.gameObject.transform.position, 10f); // this does not work, nullrefference
         //}
        //}
    }
}
