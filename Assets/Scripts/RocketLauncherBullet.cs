using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;
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
    public uint hitAvatarID;
    private Spawner spawner;
    
    private Coroutine destroyRoutine;
    [SerializeField] UnityEvent _beforeDestroy;
    private void Awake()
    {
        _transform = GetComponent<Alteruna.TransformSynchronizable>(); // might be able to use normal transform
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
            //Destroy(this.gameObject);
           //destroyRoutine = StartCoroutine(nameof(DestroyBullet));
        }

        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("bullet trigger");


       // destroyRoutine = StartCoroutine(nameof(DestroyBullet));
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
        //CustomDestroy();
        //destroyRoutine = StartCoroutine(nameof(DestroyBullet));
    }

    //[SynchronizableMethod] // yess or no?
   IEnumerator DestroyBullet() // normal
   {
      
      spawner.Despawn(this._transform.gameObject);
      yield return new WaitForSeconds(10);
      Destroy(this._transform.gameObject);

   }
   private void CustomDestroy()
   {
       _beforeDestroy.Invoke();
       Destroy(this._transform.transform.gameObject);
   }
 // private void OnDisable()
 // {
 //     StopCoroutine(nameof(DestroyBullet));
 // }

    void DoExplosion()
    {
        Debug.Log("Do Explosion");
        hitColliders =  Physics.OverlapSphere(this._transform.transform.position, 1f);
        foreach (var hitcol in hitColliders)
        {
           // avatar = hitcol.gameObject.GetComponent<Alteruna.Avatar>();
            
           // Debug.Log(avatar.Possessor.Index);
            if (hitcol.gameObject.layer == 7) // this works
            {
                Debug.Log("hello"); 
                var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
               Debug.Log(avatar);
               avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>().DoExplosion(transform.position, 1);
               
               //var p = Multiplayer.AvatarPrefab.Possessor.Index;


               // possessor.index
               // Debug.Log("Hit layer 7");
              // ProcedureParameters parameters = new ProcedureParameters();
              // parameters.Set("value", 16.0f);
              // Multiplayer.InvokeRemoteProcedure("name", avatar.Possessor.Index, parameters); // what userID?

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
