using Alteruna;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAvatarPosses : MonoBehaviour
{
    [SerializeField] private Alteruna.Avatar avatar;
    Leaderboard Leaderboard;

    private void Awake()
    {
        Leaderboard = FindObjectOfType<Leaderboard>();
    }

    public void OnPossessed(User user)
    {
        FindObjectOfType<AvatarCollection>().AddAvatar(avatar);
        Leaderboard.OnPossessed(user);
    }
}
