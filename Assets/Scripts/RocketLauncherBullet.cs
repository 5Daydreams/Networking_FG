using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RocketLauncherBullet : MonoBehaviour
{
    private TransformSynchronizable transform;
    private float speed = 10f;
    private Vector3 startPosition;

    [SerializeField] private float bulletMaxLength = 50f;

    [SerializeField] private float blastRadius = 10f;
    [SerializeField] private float explosionForce;
    
    public float hitPoints = 100.0F;
    public Collider coll;
    
    private void Awake()
    {
        transform = GetComponent<Alteruna.TransformSynchronizable>();
        startPosition = transform.transform.position;
    }

    void Start()
    {
        coll = GetComponent<Collider>();
    }
    
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        
        
        transform.transform.Translate(0, 0, speed * Time.deltaTime);
        
        if (Vector3.Distance(startPosition, this.transform.transform.position) > bulletMaxLength)
            Destroy(this.gameObject);
   
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        //Debug.Log(other.bounds.ToString());
        
     //  DoExplosion(other.transform.position, blastRadius);
     //  Debug.Log(hitPoints);
     //  Destroy(this.gameObject);
    }
    void DoExplosion(Vector3 explosionPos, float radius)
    {
        // The distance from the explosion position to the surface of the collider.
      // Vector3 closestPoint = coll.ClosestPointOnBounds(explosionPos);
      // float distance = Vector3.Distance(closestPoint, explosionPos);

      // // The damage should decrease with distance from the explosion.
      // float damage = 1.0F - Mathf.Clamp01(distance / radius);
      // hitPoints -= damage * 10.0F;
    }

    
    
}
