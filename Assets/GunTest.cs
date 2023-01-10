using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTest : MonoBehaviour
{
    [SerializeField]
    Transform ProjectileSpawnPosition;
    [SerializeField]
    GameObject Projectile;

    public Alteruna.Avatar avatar;

    // Update is called once per frame
    void Update()
    {
        if (!avatar.IsMe)
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Projectile.transform.SetPositionAndRotation(ProjectileSpawnPosition.transform.position, ProjectileSpawnPosition.transform.rotation);
            Instantiate(Projectile, ProjectileSpawnPosition.position, ProjectileSpawnPosition.rotation);
        }
    }
}
