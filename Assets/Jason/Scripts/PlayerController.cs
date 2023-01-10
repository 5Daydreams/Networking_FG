using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Base setup")]
    public float walkSpeed = 8.0f;
    public float runningSpeed = 12.0f;
    public float jumpSpeed = 10.0f;
    public float gravity = 20.0f;

    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    Transform playerInputSpace = default;


    private Alteruna.Avatar avatar;

    // Start is called before the first frame update
    void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();
        if (!avatar.IsMe)
          return;
    }

    // Update is called once per frame
    void Update()
    {
        if (!avatar.IsMe)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            controller.Move(playerInputSpace.TransformDirection(direction.x, 0f, direction.z) * walkSpeed * Time.deltaTime);
        }
    }
}
