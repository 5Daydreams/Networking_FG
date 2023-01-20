using Alteruna;
using UnityEngine;

public class UnparentByEvent : MonoBehaviour
{
    [ContextMenu("Unparent This")]
    public void RemoveParent()
    {
        this.transform.parent = null;
    }
}