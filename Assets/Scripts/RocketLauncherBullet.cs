using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;
using Avatar = UnityEngine.Avatar;

public class RocketLauncherBullet : AttributesSync
{
    [Header("Bullet object")] 
    [SerializeField] GameObject parrentObject;
    [SerializeField] MeshRenderer mesh;
    
    [Header("Bullet settings")] 
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletMaxLength = 50f;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionMaxDamage = 5f;
    [SerializeField] private float minDamage = 1f;

    [Header("Debug")] [SerializeField] private bool showExplosionRadius = true;

    [SerializeField] UnityEvent _beforeDestroy;
    
    private RigidbodySynchronizable rb;
    private Vector3 startPosition;
    private Ray ray;
    private Camera _camera;
    private bool DirectHitOnPlayer = false;
    private Collider[] hitColliders;
    private Alteruna.Avatar hitAvatar;
    
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public Spawner spawner;
    [HideInInspector] [SynchronizableField] public int UserID;

   
    private void Awake()
    {
        rb = GetComponentInParent<Alteruna.RigidbodySynchronizable>();
        startPosition = rb.transform.position;
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
    }

    void Start()
    {
        rb.velocity = direction.normalized * bulletSpeed;
        DirectHitOnPlayer = false;
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, this.rb.transform.position) > bulletMaxLength)
        {
            CustomDestroy();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DirectHitOnPlayer = false;
        if (other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>() != null)
        {
            var hitAvatar =
                other.transform.gameObject
                    .GetComponentInParent<Alteruna.Avatar>(); // so the bullet cant collide with it self
            if (hitAvatar.Possessor.Index != UserID)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    //Debug.Log("DirectHit on a player with tag");
                    DirectHitOnPlayer = true;
                }
                else if (other.gameObject.layer == 7)
                {
                    //Debug.Log("DirectHit on a player with layer");
                    DirectHitOnPlayer = true;
                }

                DoExplosion2(rb.transform.position, other);
                CustomDestroy();
            }
        }
        else
        {
            DoExplosion2(rb.transform.position, other);
            CustomDestroy();
        }
    }

    private void OnDrawGizmos()
    {
        if (showExplosionRadius)
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }

    private void CustomDestroy()
    {
        _beforeDestroy.Invoke();
        if (UserID == Multiplayer.Me.Index)
        {
            StartCoroutine(DelayedDeSpawn(1f));
            mesh.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false; // DONT ADD IT WILL BEAK
            GetComponentInParent<RigidbodySynchronizable>().SendData = false;
        }
        else
        {
            gameObject.SetActive(false);
            //Destroy(this);// DONT ADD IT WILL BEAK
        }
    }

    IEnumerator DelayedDeSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawner.Despawn(parrentObject);
        checkSpawnedObjects();
    }

    void DoExplosion2(Vector3 hitpoint, Collider other)
    {
        // Debug.Log("in avatar collection, avatar.is me [userid]: " + avatarCollection.avatars[UserID].IsMe);
        // Debug.Log(" Bullet UserID: " + UserID);
        //float l = other.transform.position.z - hitpoint.z;
        //float l = other.transform.localPosition.z - hitpoint.z;
        // Vector3 direction = other.transform.position - hitpoint;

        hitColliders = Physics.OverlapSphere(hitpoint, explosionRadius); // make a spherecast to se what is inside the explosion
        Vector3 direction = other.transform.position - hitpoint;

        if (DirectHitOnPlayer)
        {
            var hitAvatar = other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>(); hitAvatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>().AddExplosionForceOnDirectHit(direction, UserID); // Add full explosionforce to that player
        }

        foreach (var hitcol in hitColliders)
        {
            var distance = Vector3.Distance(hitpoint, hitcol.transform.position);
            var procentileDamage = explosionRadius - distance;
            float damageToDeal = procentileDamage / explosionRadius * explosionMaxDamage; // calculate the damage/force to add on the object depending on how close to the explosion it is

            if (damageToDeal < minDamage)
            {
                damageToDeal = minDamage;
            }
            Vector3 blastDir = hitcol.transform.position - this.rb.transform.position; // give a direction on the force

            if (hitcol.gameObject.CompareTag("Player")) // Layer does not work for some reason?
            {
                if (hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>() != null)
                {
                    var hitAvatarcol = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
                    if (hitAvatarcol.Possessor.Index == UserID)
                    {
                        hitAvatarcol.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                            .AddERocketJumpForce(blastDir, UserID);
                    }
                    else
                    {
                        hitAvatarcol.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                            .AddExplosionForce(damageToDeal, blastDir, UserID);
                    }
                }

                // if (hitcol.gameObject.layer == 9 && // if we want to hit otherObjects
                //     hitcol
                //         .GetComponent<
                //             RigidbodySynchronizable>()) // if the spherecast hit a object that is movable and have a RigidbodySynchronizable add a force to that object
                // {
                //     Debug.Log("The spherecast hit an object with a RigidbodySynchronizable, add force: " + damageToDeal);
                //     //Debug.Log("GameObject: " + hitcol.gameObject + ", Distance: " + distance + ", Damage: " + damageToDeal);
                //     var hitRigidbodySynchronizable = hitcol.GetComponent<RigidbodySynchronizable>();
                //     //hitRigidbodySynchronizable.velocity += Vector3.up * damageToDeal + blastDir * damageToDeal;
                //     hitRigidbodySynchronizable.AddForce(0, damageToDeal, blastDirz * damageToDeal, ForceMode.Impulse);
                // }
            }
        }
    }

    void checkSpawnedObjects()
    {
       // spawner.ForceSync = true;
       // spawner.SpawnedObjects.Clear();
       // 
       // 
       // for (int i = 0; i < spawner.SpawnedObjects.Count ; i++)
       // {
       //     spawner.ObjectSpawned<this,parrentObject>();
       //     spawner.SpawnedObjects.Remove();
       //     
       //     spawner.Despawn(parrentObject);
       // }
    }
}