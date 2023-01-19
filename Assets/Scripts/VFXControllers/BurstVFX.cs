using UnityEngine;

[RequireComponent(typeof(VFXController))]
public class BurstVFX : MonoBehaviour
{
    private VFXController _vfxSpawner;
    [SerializeField] private bool _useNetwork;
    [SerializeField] private int _indexForVFX;
    [SerializeField] private string _stringForVFX;
    [SerializeField] private DespawnBehavior _explosionPrefab;

    private void Start()
    {
        _vfxSpawner = this.GetComponent<VFXController>();
    }

    public void Spawn()
    {
        if (_useNetwork)
        {
            _vfxSpawner.SpawnVFX(Utilities.Singletons.Spawner.Instance, _indexForVFX, this.transform);
        }
        else
        {
            if (_explosionPrefab == null)
            {
                Debug.LogError("Missing the explosion prefab in the inspector");
#if UNITY_EDITOR
                UnityEngine.Application.Quit();
#else
                return;
#endif
            }

            DespawnBehavior spawnedObject = Instantiate(_explosionPrefab, this.transform.position, this.transform.rotation);
            spawnedObject.StartCountdown();
        }
    }
}