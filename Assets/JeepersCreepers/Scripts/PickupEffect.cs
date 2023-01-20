using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEffect : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float damage;
    [SerializeField] private float movementSpeed;

    public int HealthUp()
    {
        return health;
    }

    public float DamageUp()
    {
        return damage;
    }

    public float MovementSpeedUp()
    {
        return movementSpeed;
    }

}