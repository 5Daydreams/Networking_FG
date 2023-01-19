using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Alteruna;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class PlayerUiManager : AttributesSync
{
    [SerializeField] Alteruna.Avatar avatar;
    [SerializeField] Canvas canvas;

    [Header("Health")]
    [SerializeField]private PlayerHealth playerHealt;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("KDA")]
    [SerializeField] private PlayerKDA playerKDA;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI deathsText;
    [SerializeField] private TextMeshProUGUI assistText;

    [Header("Scoreboard")]
    [SerializeField] private GameObject leaderBoardUi;
    [SerializeField] public GameObject playerStats;
    [SerializeField] public TextMeshProUGUI avatarName;
    [SerializeField] public TextMeshProUGUI killScore;
    [SerializeField] public TextMeshProUGUI deathScore;
    [SerializeField] public TextMeshProUGUI assitScore;

    public Dictionary<int, GameObject> statList = new Dictionary<int, GameObject>();
    AvatarCollection avatarCollection;

    // Start is called before the first frame update
    void Start()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();

        if (avatar.IsMe)
        {
            avatarName.text = avatar.Possessor.Name;
        }
        else
            canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ToggleScoreBoard();
        UpdateKDAText();
        UpdateHealthText();
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

    public void UpdateHealthText()
    {
        healthText.text = playerHealt.GetHealth().ToString();
    }

    public void UpdateKDAText()
    {
        killScore.text = killsText.text = playerKDA.kills.ToString();
        deathScore.text = deathsText.text = playerKDA.deaths.ToString();
        assitScore.text = assistText.text = playerKDA.assist.ToString();
    }

    [SynchronizableMethod]
    public void UpdateScoreboard()
    {
        foreach (var avatar in avatarCollection.avatars)
        {
            if (!statList.ContainsKey(avatar.Key))
            {
                int i = avatar.Value.Possessor.Index;
                Alteruna.Avatar av = avatarCollection.avatars[i];
                statList.Add(i, avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().playerStats);
                Instantiate(statList[i], leaderBoardUi.transform);
            }

            PlayerStatsUi stats = avatar.Value.GetComponentInChildren<PlayerStatsUi>();
            stats.avatarName.text = avatar.Value.Possessor.Name;
            stats.killsText.text = avatar.Value.GetComponentInChildren<PlayerKDA>().kills.ToString();
            stats.deathsText.text = avatar.Value.GetComponentInChildren<PlayerKDA>().deaths.ToString();
            stats.assistText.text = avatar.Value.GetComponentInChildren<PlayerKDA>().assist.ToString();
        }
    }

    public void OnPossessed(User user)
    {
        if (avatarCollection == null)
            avatarCollection = FindObjectOfType<AvatarCollection>();

        int i = avatar.Possessor.Index;
        Alteruna.Avatar av = avatarCollection.avatars[i];

        GameObject statClone;
        statClone = Instantiate(av.GetComponentInChildren<PlayerUiManager>().playerStats, leaderBoardUi.transform);
        statList.Add(i, statClone);
    }

    IEnumerator AddToStatList()
    {
        //avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().statsInstantiated = true;

        yield return null;
    }


}
