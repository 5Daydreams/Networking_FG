using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;
using UnityEngine.UIElements;

public class RocketLauncherBullet : MonoBehaviour
{
    private TransformSynchronizable transform;
    private float speed = 10f;

    private void Awake()
    {
        transform = GetComponent<Alteruna.TransformSynchronizable>();
    }

    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        transform.transform.Translate(0,0,speed * Time.deltaTime);
        
    }
}
