using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTest : MonoBehaviour
{
    RigidbodySynchronizable rb;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    void AddForce(Vector3 Force)
    {
        rb.velocity += Force * Time.deltaTime;
    }
    void AddImpulse(Vector3 impulse)
    {
        rb.velocity += impulse;
    }

    void AddDrag(float drag)
    {
        rb.velocity *= (1 - Time.deltaTime * drag);
    }
}
