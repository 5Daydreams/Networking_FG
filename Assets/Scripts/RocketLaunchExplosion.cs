using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class RocketLaunchExplosion : MonoBehaviour
{
    private Spawner spawner;
    public Alteruna.Avatar avatar;
    private RigidbodySynchronizable rigidbodySynchronizable;

    private Vector3 ParticalSpawnPosition;
    
    [SerializeField] private float explosionForce;
    private Collider[] hitColliders;
    private float distance;
    private float blastRadius = 1f;

    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    [SerializeField] private float maxDamage;
    
    private void Awake()
    {
        //avatar = GetComponent<Alteruna.Avatar>();
       
        rigidbodySynchronizable = GetComponentInParent<RigidbodySynchronizable>();
        
      //  networkManager = GameObject.FindGameObjectsWithTag("NetWorkManager").GetComponent<Multiplayer>();
        
    }

    void Start()
    {
       
    }

    void Update()
    {
       
    }
    
    void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("RocketLauncherBullet"))
        {
            Debug.Log("Rocketbullet hit me");
           //DoExplosion(other.transform.position, blastRadius);
            //AddExplosionForce1();
           //DoExplosion(other.transform.position,1);
        }
    }
    public void AddExplosionForce1()// , float upwardsModifier, ForceMode forceMode
    {
        Vector3 upVelocity = new Vector3(0, 10, 0);
        rigidbodySynchronizable.velocity += upVelocity;
    }
    public void DoExplosion(Vector3 explosionPoint, float radius) // spelaren pos, blasradious
    {
      
        hitColliders = Physics.OverlapSphere(explosionPoint, radius);
        
        var distance = Vector3.Distance(explosionPoint, transform.position);
        var procentileDamage = distance / blastRadius;
        float damageToDeal = explosionForce * procentileDamage;
        
        Vector3 blastDir = transform.position - explosionPoint;
        
        
         if (avatar.IsMe)
         {
             Debug.Log("damageToDeal");
             Debug.Log(damageToDeal);
             
             this.rigidbodySynchronizable.velocity +=  blastDir * 2 + Vector3.up * damageToDeal; // add force to the player
         }
      //     foreach (var hitCollider in hitColliders) // add force to the the other objects around the hit
      //  {
      //      if (hitCollider.GetComponent<RigidbodySynchronizable>())
      //      {
      //          var HitRigidbodySynchronizable = hitCollider.GetComponent<RigidbodySynchronizable>();
      //          HitRigidbodySynchronizable.velocity += blastDir * 2 + Vector3.up * damageToDeal * Time.deltaTime;
      //      }
      //  }
    }
    
    float GetDamageAtPosition(Vector3 pos, Vector3 explosionPos)
    {
        float dist = Vector3.Distance(explosionPos, pos); // bomben - spelaren
        Debug.Log("dist");
        Debug.Log(dist);

        return Remap(maxRange, minRange, 0, maxDamage, dist);
    }

    // clamped remap
    static float Remap(float iMin, float iMax, float oMin, float oMax, float v)
    {
        float t = Mathf.InverseLerp(iMin, iMax, v);
        return Mathf.Lerp(oMin, oMax, t);
    }

   void DoBoom(Vector3 explosionPos)
   {
       Debug.Log("transform.position");
      Debug.Log(transform.position);
      float damage = GetDamageAtPosition(transform.position,explosionPos);
      rigidbodySynchronizable.velocity += Vector3.up * damage * Time.deltaTime;
      Debug.Log("damage");
      Debug.Log(damage);
   }
}
