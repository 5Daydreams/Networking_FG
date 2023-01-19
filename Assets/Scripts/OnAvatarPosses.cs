using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAvatarPosses : MonoBehaviour
{
    [SerializeField] private Alteruna.Avatar avatar;

    public void OnPossessed(User user)
    {
        FindObjectOfType<AvatarCollection>().AddAvatar(avatar);
    }
}
