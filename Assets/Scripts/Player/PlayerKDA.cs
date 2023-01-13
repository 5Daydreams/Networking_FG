using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alteruna;

public class PlayerKDA : AttributesSync
{
    [SynchronizableField] public int kills = 0;
    [SynchronizableField] public int deaths = 0;
    [SynchronizableField] public int assist = 0;

    public void AddKill(int amount)
    {
        kills += amount;
    }

    public void AddDeath(int amount)
    {
        deaths += amount;
    }

    public void AddAssist(int amount)
    {
        assist += amount;
    }
}
