using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using UnityEngine;

public class RocketLaunchExplosion : MonoBehaviour
{
    [Header("Player object")] 
    public Alteruna.Avatar avatar;
    [SerializeField] private PlayerHealth playerHp;
    
    [Header("Explosion settings on full hit")] 
    [SerializeField] private int fullExplosionDamageOnDirectHit = 10;
    [SerializeField] private float fullExplosionforceOnDirectHit = 6f;

    [Header("Explosion settings in radius")]
    [SerializeField] private int damageMultiplier = 2;
    
    [Header("Explosion settings for full hit/radius")]
    [SerializeField] private float upForce = 8f;
    
    [Header("Explosion settings on rocketjump")] 
    [SerializeField] private float rocketjumpForce = 5f;
    [SerializeField] private float rocketjumpUpForce = 9f;
    [SerializeField] private int rocketjumpDamage = 10;
    
    AvatarCollection avatarCollection;
    private RigidbodySynchronizable rigidbodySynchronizable;
    private int hitPlayer;

    private void Awake()
    {
        rigidbodySynchronizable = GetComponentInParent<RigidbodySynchronizable>();
        avatarCollection = FindObjectOfType<AvatarCollection>();
    }

    public void AddExplosionForce(float damage, Vector3 direction, int damageDealer) //Explosionforce depending on where on the explosnradious you get hit
    {
        if (avatar.IsMe)
        {
            //rigidbodySynchronizable.AddForce(0, upForce, direction * damage, ForceMode.Impulse);
            AddImpulse(Vector3.up * upForce + direction * damage);
            avatarCollection.avatars[damageDealer].GetComponentInChildren<PlayerHealth>().DealDamage((int)damage * damageMultiplier,playerHp);
        }
    }
    public void AddExplosionForceOnDirectHit(Vector3 direction, int damageDealer)
    {
        if (avatar.IsMe)
        {
            //rigidbodySynchronizable.AddForce(0, upForce, direction * damage, ForceMode.Impulse);
            AddImpulse(Vector3.up * upForce + direction * fullExplosionforceOnDirectHit);
            avatarCollection.avatars[damageDealer].GetComponentInChildren<PlayerHealth>().DealDamage(fullExplosionDamageOnDirectHit,playerHp);
        }
    }
    public void AddERocketJumpForce(Vector3 direction, int damageDealer)
    {
        if (avatar.IsMe)
        {
            AddImpulse(Vector3.up * rocketjumpUpForce + direction * rocketjumpForce);
            //rigidbodySynchronizable.AddForce(0, rocketjumpUpForce, direction * rocketjumpForce, ForceMode.Impulse);
            avatarCollection.avatars[damageDealer].GetComponentInChildren<PlayerHealth>().DealDamage(rocketjumpDamage,playerHp);
        }
    }
    void AddImpulse(Vector3 impulse)
    {
        rigidbodySynchronizable.velocity += impulse;
    }
}