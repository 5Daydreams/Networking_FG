using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX = 400;
    public float sensY = 400;

    public float maxClamp = 90;
    public float minClamp = -90f;

    float xRotation;
    float yRotation;

    public Transform bodyTransform;
    public Transform gunSocketTransform;

    public Alteruna.Avatar avatar;

    void Start()
    {
        if (!avatar.IsMe)
        {
            GetComponent<Camera>().enabled = false;
            return;
        }

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        if (!avatar.IsMe)
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minClamp, maxClamp);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        bodyTransform.transform.right = gameObject.transform.right;

        CursorLock();
    }

    void CursorLock()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
