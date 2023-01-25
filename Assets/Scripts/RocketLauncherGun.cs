using Alteruna;
using UnityEngine;

public class RocketLauncherGun : AttributesSync
{
    [Header("Gun object")]
    [SerializeField] Transform gunPipe;
    [SerializeField] private Camera _camera;
    [SerializeField] PlayerHealth playerHealth;
    public Alteruna.Avatar avatar;
    
    [Header("Gun settings")] 
    [SerializeField] private float maxRayLenght = 1000f;
    [SerializeField] private float maxBulletLength = 100f;
    [SerializeField] private float shootTimer = 3f;
    [SerializeField] private int indexToSpawn = 0;
    
    private float defaultShootTimer = 3f;
    private bool IsRealoading = false;
    private Ray ray;
    private Vector3 HitPoint;
    private Spawner spawner;
    private TeamManager teamManager;

    public bool bJoinedTeam = false;

    private void Awake()
    {
        spawner = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<Spawner>();
        defaultShootTimer = shootTimer;
    }

    void Update()
    {
        if (!avatar.IsMe)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0) && !IsRealoading && !playerHealth.dead && bJoinedTeam)
        {
            SpawnBullet();
            IsRealoading = true;
        }

        if (IsRealoading)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                IsRealoading = false;
                shootTimer = defaultShootTimer;
            }
        }
    }
    void SpawnBullet()
    {
        RaycastHit Hit;
        ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(ray, out Hit, maxRayLenght))
        {
            HitPoint = Hit.point;
        }
        else
        {
            HitPoint = ray.GetPoint(maxBulletLength);
        }

        GameObject bullet = spawner.Spawn(indexToSpawn, gunPipe.position + gunPipe.forward, gunPipe.rotation);
        bullet.GetComponentInChildren<RocketLauncherBullet>().UserID = Multiplayer.Me.Index;
        bullet.GetComponentInChildren<RocketLauncherBullet>().direction = HitPoint - gunPipe.position;
    }
}