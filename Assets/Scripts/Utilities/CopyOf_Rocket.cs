using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;
using Avatar = UnityEngine.Avatar;

public class CopyOf_Rocket : MonoBehaviour
{
    private TransformSynchronizable transform;
    private Vector3 startPosition;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletMaxLength = 50f;

   // public float hitPoints = 100.0F;
    public SphereCollider coll;

    private LayerMask playerLayer;
    private Collider[] hitColliders;
    
    [SerializeField] UnityEvent _beforeDestroy;
    
    private void Awake()
    {
        transform = GetComponent<Alteruna.TransformSynchronizable>();
        startPosition = transform.transform.position;
    }

    void Start()
    {
       coll = GetComponent<SphereCollider>();
    }

    private void CustomDestroy()
    {
        _beforeDestroy.Invoke();
        Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
    }
    
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        transform.transform.Translate(0, 0, bulletSpeed * Time.deltaTime);

        if (Vector3.Distance(startPosition, this.transform.transform.position) > bulletMaxLength)
        {
            CustomDestroy();
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("bullet trigger");
        //hitColliders =  Physics.OverlapSphere(this.transform.transform.position, 10f, playerLayer);
        //DoExplosion();
        CustomDestroy();
    }
    void DoExplosion()
    {
        foreach (Collider hitcol in hitColliders)
        {
         // Debug.Log("inside foreach loop");
         //  if (hitcol.gameObject.layer == 7) // this works
         //  {
         //      Debug.Log(hitcol.gameObject.layer );
         //      Debug.Log("hitcol.gameObject.layer");
         //     
         //      hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(hitcol.gameObject.transform.position, 10f); // this does not work, nullrefference
         //  }
         //  if (hitcol.gameObject.CompareTag("Player"))
         //  {
         //      Debug.Log("bullet trigger gameobject Player");
         //      hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(gameObject.transform.position, 10f);
         //  }
         //  if (hitcol.CompareTag("Player"))
         //  {
         //      Debug.Log("bullet trigger Player");
         //      hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(gameObject.transform.position, 10f);
         //  }

         // if (playerLayer == 7)
         // {
         //     Debug.Log(playerLayer);
         //     Debug.Log("hitcol.gameObject.layer");
         //    
         //     hitcol.GetComponent<RocketLaunchExplosion>().DoExplosion(hitcol.gameObject.transform.position, 10f); // this does not work, nullrefference
         // }
        }
    }
}
