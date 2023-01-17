using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDebug : MonoBehaviour
{
    [ContextMenu("Destroy This")]
    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        this.GetComponentInChildren<UnparentByEvent>().RemoveParent();
    }
}
