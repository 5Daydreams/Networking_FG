using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class RocketLaunchExplosion : MonoBehaviour
{
    private Spawner spawner;
    private Alteruna.Avatar avatar;
    private RigidbodySynchronizable rigidbodySynchronizable;

    private Vector3 ParticalSpawnPosition;
    
    [SerializeField] private float explosionForce;
    private Collider[] hitColliders;
    private float distance;
    private float blastRadius = 5f;

    [SerializeField] private float minRange;
    [SerializeField] private float maxRange;
    [SerializeField] private float maxDamage;
    
    public Multiplayer networkManager;
    

    private void Awake()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        rigidbodySynchronizable = GetComponent<Alteruna.RigidbodySynchronizable>();
      //  networkManager = GameObject.FindGameObjectsWithTag("NetWorkManager").GetComponent<Multiplayer>();
        
    }

    void Start()
    {
        networkManager.RegisterRemoteProcedure("name",AddExplosionForce);
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
           AddExplosionForce1();
        }
    }
    public void AddExplosionForce1()// , float upwardsModifier, ForceMode forceMode
    {
        Vector3 upVelocity = new Vector3(0, 10, 0);
        rigidbodySynchronizable.velocity += upVelocity;
    }
    public void AddExplosionForce(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)// , float upwardsModifier, ForceMode forceMode
    {
        Vector3 upVelocity = new Vector3(0, 500, 0);
        rigidbodySynchronizable.velocity += upVelocity;
    }
    public void DoExplosion(Vector3 explosionPoint, float radius) // spelaren pos, blasradious
    {
        hitColliders = Physics.OverlapSphere(explosionPoint, radius);
        
        var distance = Vector3.Distance(explosionPoint, transform.position);
        var procentileDamage = distance / blastRadius;
        float damageToDeal = explosionForce * procentileDamage;
        
        Vector3 blastDir = transform.position - explosionPoint;
        
        Debug.Log("damageToDeal");
        Debug.Log(damageToDeal);
        
        rigidbodySynchronizable.velocity +=  blastDir * 2 + Vector3.up * damageToDeal  *Time.deltaTime; // add force to the player
      // foreach (var hitCollider in hitColliders) // add force to the the other objects around the hit
      // {
      //     if (hitCollider.GetComponent<RigidbodySynchronizable>())
      //     {
      //         var HitRigidbodySynchronizable = hitCollider.GetComponent<RigidbodySynchronizable>();
      //         HitRigidbodySynchronizable.velocity += blastDir * 2 + Vector3.up * damageToDeal * Time.deltaTime;
      //     }
      // }
        
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
