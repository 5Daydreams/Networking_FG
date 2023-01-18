using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class RocketLaunchExplosion : MonoBehaviour
{
    //private Spawner spawner;
    public Alteruna.Avatar avatar;
    private RigidbodySynchronizable rigidbodySynchronizable;

    //private Vector3 ParticalSpawnPosition;
    
    private float fullExplosionDamage;
  // private Collider[] hitColliders;
  // private float distance;
  // private float blastRadius = 1f;
    
    private int hitPlayer;
    
    private void Awake()
    {
        rigidbodySynchronizable = GetComponentInParent<RigidbodySynchronizable>();
    }

    void Start()
    {
       
    }

    void Update()
    {
       
    }
    
    void OnTriggerEnter(Collider other)
    {
      // Debug.Log("collided with something");
      // if (other.CompareTag("RocketLauncherBullet"))
      // {
      //     Debug.Log("collided with bullet");
      //     var spawner = other.GetComponent<RocketLauncherBullet>().spawner;
      //     spawner.Despawn(other.gameObject);
      // }
    }
    //public void AddExplosionForce1()// , float upwardsModifier, ForceMode forceMode
    //{
    //    Vector3 upVelocity = new Vector3(0, 10, 0);
    //    rigidbodySynchronizable.velocity += upVelocity;
    //}
    //public void DoExplosion(Vector3 explosionPoint, float radius) // spelaren pos, blasradious
    //{
    //    var distance = Vector3.Distance(explosionPoint, transform.position);
    //   var procentileDamage = distance / blastRadius;
    //   float damageToDeal = explosionForce * procentileDamage;
    //    
    //   Vector3 blastDir = transform.position - explosionPoint;
//
    //   if (avatar.IsMe)
    //   {
    //       //Debug.Log("damageToDeal on player: " + damageToDeal);
    //       
    //       this.rigidbodySynchronizable.velocity +=  blastDir * 2 + Vector3.up * damageToDeal; // add force to the player
    //       
    //       hitColliders = Physics.OverlapSphere(explosionPoint, radius);
    //       foreach (var hitCollider in hitColliders) // add force to the the other objects around the hit
    //       {
    //      
    //           if (hitCollider.GetComponent<RigidbodySynchronizable>())
    //           {
    //               //Debug.Log("GameObject: " + hitCollider.gameObject + ", Distance: " + distance + ", Damage: " + damageToDeal );
    //               var hitRigidbodySynchronizable = hitCollider.GetComponent<RigidbodySynchronizable>();
    //               hitRigidbodySynchronizable.velocity += blastDir * 2 + Vector3.up * damageToDeal * Time.deltaTime;
    //           }
    //       }
    //   }
    //}

    public void AddExplosionForce(Vector3 explosionPoint, float damage, Vector3 direction) 
    {
        if (avatar.IsMe)
        {
            Debug.Log("Add force to player i hit with bullet:" + damage);
            this.rigidbodySynchronizable.velocity +=  Vector3.up * damage + direction * damage;
            Debug.Log("Add force to player");
        }
    }
}
