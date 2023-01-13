using System.Collections;
using System.Collections.Generic;
using Alteruna;

using UnityEngine;

public class UPDATEHUEPLEASE : AttributesSync
{
    public Material blueMaterial;
    public MeshRenderer renderer;
    MeshRenderer[] epic = new MeshRenderer[0];
    
    [SynchronizableField]
    UniqueAvatarColor color = new UniqueAvatarColor();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            epic[0] = renderer;
            color.meshes = epic;
            renderer.material = blueMaterial;
            color.UpdateHue();
        }
    }
}
