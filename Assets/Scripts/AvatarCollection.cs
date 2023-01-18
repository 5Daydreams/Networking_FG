using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCollection : MonoBehaviour
{
    public Dictionary<int ,Alteruna.Avatar> avatars = new Dictionary<int, Alteruna.Avatar>();

    public void AddAvatar(Alteruna.Avatar avatar)
    {
        avatars.Add(avatar.Possessor.Index, avatar);
    }

    public void RemoveOnOtherLeave(Multiplayer multiplayer, User user)
    {
        Debug.Log("Remove");
        avatars.Remove(user.Index);
    }

    public void RemoveOnLeave(Multiplayer multiplayer)
    {
        Alteruna.Avatar me;
        me = avatars[multiplayer.Me.Index];
        Debug.Log("Player left");
        avatars.Clear();
        //AddAvatar(me);
    }
}
