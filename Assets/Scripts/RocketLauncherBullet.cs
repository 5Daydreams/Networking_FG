using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.Events;
using Avatar = UnityEngine.Avatar;

public class RocketLauncherBullet : AttributesSync
{
    //private TransformSynchronizable _transform;
    private RigidbodySynchronizable rb;
    private Vector3 startPosition;
    private Ray ray;
    private Camera camera;
    public Vector3 direction;

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletMaxLength = 50f;
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float fullExplosionforceOnDirectHit = 4f;
    [SerializeField] private float explosionForce = 5f;
    [SerializeField] private float explosionForceRocketJump = 5f;
    
    
    AvatarCollection avatarCollection;

    private bool DirectHitOnPlayer = false;
    //[SerializeField] private float bulletDamage = 5f;

    // public float hitPoints = 100.0F;
    //public SphereCollider coll;

    private LayerMask playerLayer;
    private Collider[] hitColliders;
    
    private Coroutine destroyRoutine;
    [SerializeField] UnityEvent _beforeDestroy;

    [HideInInspector]
    public Spawner spawner;
    
    [HideInInspector]
    [SynchronizableField] public int UserID;

    [SerializeField]
    private bool showExplosionRadius = true;

    private void Awake()
    {
       // _transform = GetComponentInParent<Alteruna.TransformSynchronizable>(); // might be able to use normal transform
        rb = GetComponentInParent<Alteruna.RigidbodySynchronizable>();
        startPosition = rb.transform.position;
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        
        //camera = GetComponentInParent<RocketLauncherGun>().camera;
        //camera = avatarCollection.avatars[UserID].GetComponentInParent<RocketLauncherGun>().camera;
        
    }

    void Start()
    {
        Debug.Log("Multiplayer.Me.Index in bullet: " + Multiplayer.Me.Index);
        Debug.Log("UserID in bullet in bullet: " +UserID);
        avatarCollection = FindObjectOfType<AvatarCollection>();

       
        
        //ray = GetComponentInParent<RocketLauncherGun>().ray;
        rb.velocity = direction.normalized * bulletSpeed;
        DirectHitOnPlayer = false;
    }

    private void FixedUpdate()
    {
       
       // AddForce(transform.forward * bulletSpeed);
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
         if (other.gameObject.CompareTag("Player"))
         {
             Debug.Log("DirectHit on a player");
             DirectHitOnPlayer = true;
         }
       
        DoExplosion2(rb.transform.position, other);
        CustomDestroy();
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
            StartCoroutine(DelayedDeSpawn (1f));
            GetComponentInParent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false; // DONT ADD IT WILL BEAK
          //  GetComponentInParent<RigidbodySynchronizable>().SendData = false;
            // _beforeDestroy.Invoke();
        }
        else
        {
            gameObject.SetActive(false);
            // Destroy(this);// DONT ADD IT WILL BEAK
        }
    }

    IEnumerator DelayedDeSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawner.Despawn(transform.gameObject);
    }

    void DoExplosion2(Vector3 hitpoint, Collider other)
    {
        Debug.Log("in avatar collection, avatar.is me [userid]: " +avatarCollection.avatars[UserID].IsMe);
        Debug.Log(" Bullet UserID: " + UserID);
        
        
        hitColliders = Physics.OverlapSphere(hitpoint, explosionRadius); // make a spherecast to se what is inside the explosion
        
       if (DirectHitOnPlayer)
       {
           Debug.Log("Add Full force to player i hit");
           var hitAvatar = other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
           hitAvatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>().AddExplosionForce(hitpoint, fullExplosionforceOnDirectHit, other.transform.position - hitpoint); // Add full explosionforce to that player
       }
       
        foreach (var hitcol in hitColliders)
        {
            var distance = Vector3.Distance(hitpoint, hitcol.transform.position);
            var procentileDamage = explosionRadius - distance;
            float damageToDeal = procentileDamage / explosionRadius * explosionForce; // calculate the damage/force to add on the object depending on how close to the explosion it is

            Vector3 blastDir = hitcol.transform.position - this.rb.transform.position; // give a direction on the force

            if (hitcol.gameObject.CompareTag("Player")) // Layer does not work for some reason?
            {
                if (!DirectHitOnPlayer &&
                    !avatarCollection.avatars[UserID]
                        .IsMe) // the explosion hit a player and we did not get a direct hit. And the id is not the same as the person that shot the bullet
                {
                    Debug.Log("The spherecast hit a playertag");
                    Debug.Log("The spherecast hit a player, distance: " + distance);
                    var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
                    avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                        .AddExplosionForce(hitpoint, damageToDeal, blastDir);
                }
            }

            //if (avatarCollection.avatars[UserID].IsMe)
             //{
             //    avatarCollection.avatars[UserID].GetComponentInChildren<>()
             //}
           //  if (DirectHitOnPlayer == false && UserID != Multiplayer.Me.Index)
           //  {
           //      Debug.Log("The spherecast hit a player, distance: " + distance);
           //      var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>(); 
           //      avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>().AddExplosionForce(hitpoint, damageToDeal, blastDir);
           //  }
           //  if (UserID == Multiplayer.Me.Index)
           //  {
           //      Debug.Log("UserID:" + UserID);
           //      Debug.Log("UMultiplayer.Me:" + Multiplayer.Me.Index);
           //      Debug.Log("The spherecast hit me, add force for rocket jump: " + explosionForceRocketJump);
           //      var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>(); 
           //      avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>().AddExplosionForce(hitpoint, explosionForceRocketJump, blastDir); // use a set rocketjumpforce or use the calculated one?
           //  }
          
          if (hitcol.gameObject.layer == 9 && hitcol.GetComponent<RigidbodySynchronizable>()) // if the spherecast hit a object that is movable and have a RigidbodySynchronizable add a force to that object
          {
              Debug.Log("The spherecast hit an object with a RigidbodySynchronizable, add force: " + damageToDeal);
               //Debug.Log("GameObject: " + hitcol.gameObject + ", Distance: " + distance + ", Damage: " + damageToDeal);
               var hitRigidbodySynchronizable = hitcol.GetComponent<RigidbodySynchronizable>();
               hitRigidbodySynchronizable.velocity += Vector3.up * damageToDeal + blastDir * damageToDeal;
          }
        }
    }
    void AddForce(Vector3 Force)
    {
        rb.velocity += Force * Time.deltaTime;
    }
}