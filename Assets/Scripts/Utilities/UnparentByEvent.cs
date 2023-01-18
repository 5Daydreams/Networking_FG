using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

[RequireComponent(typeof(TransformSynchronizable))]
public class UnparentByEvent : MonoBehaviour
{
    private TransformSynchronizable _transform;

    private void Awake()
    {
        _transform = this.GetComponent<TransformSynchronizable>();
    }

    [ContextMenu("Unparent This")]
    public void RemoveParent()
    {
        this._transform.transform.parent = null;
    }
}