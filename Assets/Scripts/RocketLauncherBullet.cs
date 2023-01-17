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

    //public Multiplayer NetworkManager;
    public Alteruna.Avatar avatar;
    private Spawner spawner;
    private void Awake()
    {
        _transform = GetComponent<Alteruna.TransformSynchronizable>();
        startPosition = _transform.transform.position;
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
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
        {
            DestroyBullet();
        }

        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("bullet trigger");
        
        
        DestroyBullet();
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

        //DoExplosion();
        //Destroy(this.gameObject);
        
    }

    void DestroyBullet()
    {
        spawner.Despawn(this.gameObject);
            
        Destroy(this.gameObject);
    }

    void DoExplosion()
    {
        hitColliders =  Physics.OverlapSphere(this._transform.transform.position, 10f);
        foreach (var hitcol in hitColliders)
        {
            avatar = hitcol.gameObject.GetComponent<Alteruna.Avatar>();
            
            Debug.Log(avatar.Possessor.Index);
            if (hitcol.gameObject.layer == 7) // this works
            {
               
                ProcedureParameters parameters = new ProcedureParameters();
                
                parameters.Set("value", 16.0f);
                Multiplayer.InvokeRemoteProcedure("name", avatar.Possessor.Index, parameters); // Null
               
              //  Debug.Log(hitcol.gameObject.layer );
              //  Debug.Log("hitcol.gameObject.layer");
              //  Debug.Log(hitcol.gameObject);
              //  PlayerHealth playerHp = hitcol.transform.GetComponentInChildren<PlayerHealth>();
              //  Debug.Log("playerHp");
              //  Debug.Log(playerHp);
              
                
               ///// NULL
                // avatar.gameObject.GetComponent<RocketLaunchExplosion>()
                //             .DoExplosion(avatar.gameObject.transform.position, 10f);
                // avatar.Possessor.Index
                // 
                //hitcol.gameObject.GetComponent<RocketLaunchExplosion>().AddExplosionForce();
               //hitcol.GetComponent<RocketLaunchExplosion>().AddExplosionForce(); // this does not work, nullrefference
            }
        }
          
    }
}
