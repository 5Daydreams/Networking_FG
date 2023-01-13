using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class JesperPlayerController : AttributesSync
{
    private float movementSpeed = 10f;
    private Alteruna.Avatar avatar;
    
    [SynchronizableField]
    public int team = -1;

    [SynchronizableMethod]
    public void SetMaterial(Material material) { GetComponent<MeshRenderer>().material = material; }

    void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        if (avatar.IsMe == false) { return; }

    }

    void Update()
    {
        if (avatar.IsMe == false) { return; }
        HandleMovement();
    }

    void HandleMovement()
    {

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.forward * -movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.right * -movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
    }
}

