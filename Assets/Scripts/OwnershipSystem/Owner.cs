using Alteruna;
using UnityEngine;

public class Owner : MonoBehaviour
{
    private bool dirtyTag_IdSet = false;
    private int _id = -1;

    public int ID
    {
        get => _id;

        set
        {
            if (dirtyTag_IdSet)
            {
                Debug.LogError("Attempted to own an  already owned object");
                return;
            }

            dirtyTag_IdSet = true;
            _id = value;
        }
    }
}