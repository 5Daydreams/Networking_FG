using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Singleton Error - found pre-existing singleton within scene.\n" +
                           "Despawning the entirety of the following GAME OBJECT: " + this.gameObject.name +
                           ", as well as it's components.");
            Utilities.Singletons.Spawner.Instance.Despawn(this.gameObject);
            // Destroy(this);
        }
    }
}