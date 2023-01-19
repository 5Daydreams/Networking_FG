using Alteruna;
using UnityEngine;

namespace Utilities.Singletons
{
    [RequireComponent(typeof(Multiplayer), typeof(Owner), typeof(Alteruna.Spawner))]
    public class Spawner : Singleton<ExtendedSpawner>
    {

    }
}