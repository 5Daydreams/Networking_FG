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
    [SerializeField] private float fullExplosionforceOnDirectHit = 6f;
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

    [HideInInspector] public Spawner spawner;

    [HideInInspector] [SynchronizableField]
    public int UserID;

    [SerializeField] private bool showExplosionRadius = true;
    [SerializeField] private LayerMask playerselfLayer;

    [SerializeField] GameObject parrentObject;
    [SerializeField] MeshRenderer mesh;
    private Alteruna.Avatar hitAvatar;
   


    private void Awake()
    {
        rb = GetComponentInParent<Alteruna.RigidbodySynchronizable>();
        startPosition = rb.transform.position;
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
    }

    void Start()
    {
        // Debug.Log("Multiplayer.Me.Index in bullet: " + Multiplayer.Me.Index);
        // Debug.Log("UserID in bullet in bullet: " +UserID);

        avatarCollection = FindObjectOfType<AvatarCollection>();
        rb.velocity = direction.normalized * bulletSpeed;
        DirectHitOnPlayer = false;
    }

    private void FixedUpdate()
    {
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
        //if (other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>()!= null)
        //{
        //    hitAvatar = other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>(); // so the bullet cant collide with it self
        //}
        
       if (other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>()!= null) 
       {
            var hitAvatar = other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>(); // so the bullet cant collide with it self
            if ( hitAvatar.Possessor.Index != UserID)
            {
                DirectHitOnPlayer = false;
                if (other.gameObject.CompareTag("Player"))
                {
                    Debug.Log("DirectHit on a player with tag");
                    DirectHitOnPlayer = true;
                }
                else if (other.gameObject.layer == 7)
                {
                    Debug.Log("DirectHit on a player with layer");
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
        //else if(other.transform.gameObject.CompareTag("Ground"))
        //{
        //    DoExplosion2(rb.transform.position, other);
        //    CustomDestroy();
        //}
        
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
            // Destroy(this);// DONT ADD IT WILL BEAK
        }
    }

    IEnumerator DelayedDeSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawner.Despawn(parrentObject);
    }

    void DoExplosion2(Vector3 hitpoint, Collider other)
    {
       // Debug.Log("in avatar collection, avatar.is me [userid]: " + avatarCollection.avatars[UserID].IsMe);
       // Debug.Log(" Bullet UserID: " + UserID);

        hitColliders = Physics.OverlapSphere(hitpoint, explosionRadius); // make a spherecast to se what is inside the explosion
        //float l = other.transform.position.z - hitpoint.z;
        //float l = other.transform.localPosition.z - hitpoint.z;
       // Vector3 direction = other.transform.position - hitpoint;
        Vector3 direction = other.transform.position - hitpoint;

        if (DirectHitOnPlayer)
        {
            Debug.Log("Add Full force to player i hit");
            var hitAvatar = other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
            hitAvatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                .AddExplosionForce(hitpoint, fullExplosionforceOnDirectHit,
                    direction,UserID); // Add full explosionforce to that player
        }

        foreach (var hitcol in hitColliders)
        {
          var distance = Vector3.Distance(hitpoint, hitcol.transform.position);
          var procentileDamage = explosionRadius - distance;
          float damageToDeal = procentileDamage / explosionRadius * explosionForce; // calculate the damage/force to add on the object depending on how close to the explosion it is

          Vector3 blastDir = hitcol.transform.position - this.rb.transform.position; // give a direction on the force
          //float blastDirz = blastDir.z;

          if (hitcol.gameObject.CompareTag("Player")) // Layer does not work for some reason?
          {
              if (avatarCollection.avatars[UserID].Possessor.Index == UserID) // the explosion hit a player and we did not get a direct hit. And the id is not the same as the person that shot the bullet
               {
                  Debug.Log("The spherecast hit me");
                  var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
                  avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>().AddERocketJumpForce(hitpoint,blastDir);
               }

              if (!DirectHitOnPlayer && avatarCollection.avatars[UserID]
                      .Possessor.Index != UserID)
              {
                  Debug.Log("The spherecast hit a playertag");
                  Debug.Log("The spherecast hit a player, distance: " + distance);
                  var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
                  avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                      .AddExplosionForce(hitpoint, damageToDeal, direction,UserID);
              }
          }

          // if (hitcol.gameObject.layer == 9 &&
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

    void AddForce(Vector3 Force)
    {
        rb.velocity += Force * Time.deltaTime;
    }
}