using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

[RequireComponent(typeof(UniqueAvatarColor))]
public class TeamColorChoose : MonoBehaviour
{
    [SerializeField] private Color[] _teamColors;
    public int _myTeam = 3;
    private UniqueAvatarColor _uaColor;

    private void Awake()
    {
        _uaColor = this.GetComponent<UniqueAvatarColor>();

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mpb.SetColor("_BaseColor", _teamColors[_myTeam]);

        foreach (MeshRenderer renderer in _uaColor.meshes)
        {
            renderer.SetPropertyBlock(mpb);
        }
        
        _uaColor.UpdateHue();
    }
}