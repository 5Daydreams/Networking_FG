using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAvatarPosses : MonoBehaviour
{
    [SerializeField] private Alteruna.Avatar avatar;
    [SerializeField] PlayerUiManager uiManager;

    public void OnPossessed(User user)
    {
        FindObjectOfType<AvatarCollection>().AddAvatar(avatar);
        //uiManager.OnPossessed(user);
    }
}
