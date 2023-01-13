using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] Alteruna.Avatar avatar;
    [SerializeField] Canvas canvas;

    void Start()
    {
        if (!avatar.IsMe)
            canvas.enabled = false;
    }
}
