using Alteruna;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Leaderboard : AttributesSync
{
    [SerializeField] private PlayerKDA playerKDA;

    [Header("Scoreboard")]
    [SerializeField] private GameObject leaderBoardUi;
    [SerializeField] public GameObject playerStats;

    public Dictionary<int, GameObject> statList = new Dictionary<int, GameObject>();
    AvatarCollection avatarCollection;

    private void Awake()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();

    }

    // Update is called once per frame
    void Update()
    {
        ToggleScoreBoard();
    }

    void ToggleScoreBoard()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            leaderBoardUi.SetActive(true);
            BroadcastRemoteMethod("UpdateScoreboard");
        }
        if (Input.GetKeyUp(KeyCode.Tab))
            leaderBoardUi.SetActive(false);
    }

    [SynchronizableMethod]
    public void UpdateScoreboard()
    {
        foreach (var avatar in avatarCollection.avatars)
        {
            int i = avatar.Value.Possessor.Index;
            Alteruna.Avatar av = avatarCollection.avatars[i];
            GameObject statClone;

            if (!statList.ContainsKey(avatar.Key))
            {
                statClone = Instantiate(playerStats, leaderBoardUi.transform);
                statList.Add(i, statClone);
                statClone.name = avatar.Value.Possessor.Name + " Stats";
            }

            PlayerStatsUi stats = statList[avatar.Key].GetComponentInChildren<PlayerStatsUi>();
            stats.avatarName.text = avatar.Value.Possessor.Name;
            stats.killsText.text = avatar.Value.GetComponentInChildren<PlayerKDA>().kills.ToString();
            stats.deathsText.text = avatar.Value.GetComponentInChildren<PlayerKDA>().deaths.ToString();
            stats.assistText.text = avatar.Value.GetComponentInChildren<PlayerKDA>().assist.ToString();
        }
    }

    public void OnPossessed(User user)
    {
        BroadcastRemoteMethod("UpdateScoreboard");
    }
}
