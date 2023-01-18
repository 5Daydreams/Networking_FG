using System;
using System.Collections.Generic;
using Alteruna;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Utilities.Singletons
{
    [RequireComponent(typeof(Multiplayer), typeof(Owner), typeof(Alteruna.Spawner))]
    public class Spawner : Singleton<ExtendedSpawner>
    {

    }
}