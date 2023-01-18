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

    [SerializeField] private float explosionRadius = 10f;

    [SerializeField] private float fullExplosionforce = 2f;
    //[SerializeField] private float bulletDamage = 5f;

    // public float hitPoints = 100.0F;
    public SphereCollider coll;

    private LayerMask playerLayer;
    private Collider[] hitColliders;

    //public Multiplayer NetworkManager;
    public Spawner spawner;

    private DespawnBehavior _despawner;
    private Coroutine destroyRoutine;
    [SerializeField] UnityEvent _beforeDestroy;

    [HideInInspector] [SynchronizableField]
    public int UserID;

    private void Awake()
    {
        _despawner = this.transform.GetComponentInChildren<DespawnBehavior>();
        _transform = GetComponent<Alteruna.TransformSynchronizable>(); // might be able to use normal transform
        startPosition = _transform.transform.position;
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
    }

    void Start()
    {
        //coll = GetComponent<SphereCollider>();
        Debug.Log("UserID in bullet: " + UserID);
    }

    private void FixedUpdate()
    {
    }

    void Update()
    {
        _transform.transform.Translate(0, 0, bulletSpeed * Time.deltaTime);

        if (Vector3.Distance(startPosition, this._transform.transform.position) > bulletMaxLength)
        {
            CustomDestroy();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DoExplosion2(_transform.transform.position, other);
        CustomDestroy();
    }

    private void CustomDestroy()
    {
        // Debug.Log("Multiplayer.Me in bullet: " + Multiplayer.Me.Index);
        // Debug.Log("UserID in bullet: " + UserID);
        if (UserID == Multiplayer.Me.Index)
        {
            _despawner.SetOwner(this.UserID);
            _beforeDestroy.Invoke();
            spawner.Despawn(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator DelayedDeSpawne(float delay)
    {
        yield return new WaitForSeconds(delay);
        spawner.Despawn(transform.gameObject);
    }

    void DoExplosion2(Vector3 hitpoint, Collider other)
    {
        hitColliders =
            Physics.OverlapSphere(hitpoint, explosionRadius); // make a spherecast to se what is inside the explosion

        foreach (var hitcol in hitColliders)
        {
            var distance = Vector3.Distance(hitpoint, hitcol.transform.position);
            var procentileDamage = explosionRadius - distance;
            float damageToDeal =
                procentileDamage / explosionRadius *
                fullExplosionforce; // calculate the damage/force to add on the object depending on how close to the explosion it is

            Vector3 blastDir =
                hitcol.transform.position - this._transform.transform.position; // give a direction on the force

            if (other.gameObject.GetComponentInParent<Alteruna.Avatar>() != null &&
                hitcol.gameObject.layer == 7) // if the object we collide with have a avatar & is a player
            {
                Debug.Log("Add fullforce to player");
                var hitAvatar = other.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
                hitAvatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                    .AddExplosionForce(hitpoint, fullExplosionforce,
                        -blastDir); // Add full explosionforce to that player

                if (other.gameObject.GetComponentInParent<Alteruna.Avatar>().Possessor.Index ==
                    hitcol.gameObject.GetComponentInParent<Alteruna.Avatar>().Possessor
                        .Index) // if the spherecast hit something with the same index as we hit the bullet, continue
                {
                    Debug.Log("continue");
                    continue;
                }
            }
            // else if (other.gameObject.GetComponent<RigidbodySynchronizable>() != null && other.gameObject.layer == 9)
            // {
            //     Debug.Log("Direct hit on something with layer 9");
            //    var hitRigidbodySynchronizable = other.GetComponent<RigidbodySynchronizable>();
            //    hitRigidbodySynchronizable.velocity += Vector3.up * fullExplosionforce + -blastDir * fullExplosionforce;
            //    Debug.Log("fullExplosionforce: " + fullExplosionforce);
            //   
            //  // if (other.gameObject.GetInstanceID() == hitcol.gameObject.GetInstanceID())
            //  // {
            //  //     Debug.Log("continue");
            //  //     continue;
            //  // }
            // }

            if (hitcol.gameObject.layer == 7 &&
                UserID != Multiplayer.Me
                    .Index) // if the spherecast hit a player but its not me add a force tot hat player
            {
                Debug.Log("The spherecast hit a player");
                //Debug.Log("GameObjectPlayer: " + hitcol.gameObject + ", Distance: " + distance + ", Damage: " + damageToDeal);
                var avatar = hitcol.transform.gameObject.GetComponentInParent<Alteruna.Avatar>();
                avatar.transform.gameObject.GetComponentInChildren<RocketLaunchExplosion>()
                    .AddExplosionForce(hitpoint, damageToDeal, blastDir);
            }

            if (hitcol.gameObject.layer ==
                9) // if the spherecast hit a object that is movable and have a RigidbodySynchronizable add a force to that object
            {
                if (hitcol.GetComponent<RigidbodySynchronizable>())
                {
                    Debug.Log("The spherecast hit an object with a RigidbodySynchronizable");
                    //Debug.Log("GameObject: " + hitcol.gameObject + ", Distance: " + distance + ", Damage: " + damageToDeal);
                    var hitRigidbodySynchronizable = hitcol.GetComponent<RigidbodySynchronizable>();
                    hitRigidbodySynchronizable.velocity += Vector3.up * damageToDeal + blastDir * damageToDeal;
                }
            }
        }
    }
}