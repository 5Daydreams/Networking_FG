using UnityEngine;

[RequireComponent(typeof(VFXController))]
public class BurstVFX : MonoBehaviour
{
    private VFXController _vfxSpawner;
    [SerializeField] private int _indexForVFX;
    [SerializeField] private string _stringForVFX;

    private void Start()
    {
        _vfxSpawner = this.GetComponent<VFXController>();
    }

    public void Spawn()
    {
        _vfxSpawner.SpawnVFX(Utilities.Singletons.Spawner.Instance, _indexForVFX, this.transform);
    }
}