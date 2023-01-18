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
    public bool statsInstantiated = false;

    public List<GameObject> statList = new List<GameObject>();
    AvatarCollection avatarCollection;

    // Start is called before the first frame update
    void Start()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();

        if (avatar.IsMe)
        {
            avatarName.text = avatar.Possessor.Name;
            //statList.Add(playerStats);
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
        for (int i = 0; i < avatarCollection.avatars.Count; i++)
        {
            avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().avatarName.text = avatarCollection.avatars[i].Possessor.Name;
            avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().killScore.text = avatarCollection.avatars[i].GetComponentInChildren<PlayerKDA>().kills.ToString();
            avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().deathScore.text = avatarCollection.avatars[i].GetComponentInChildren<PlayerKDA>().deaths.ToString();
            avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().assitScore.text = avatarCollection.avatars[i].GetComponentInChildren<PlayerKDA>().assist.ToString();

            if (!avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().statsInstantiated)
            {
                statList.Add(avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().playerStats);
                Instantiate(statList[i], leaderBoardUi.transform);
                avatarCollection.avatars[i].GetComponentInChildren<PlayerUiManager>().statsInstantiated = true;
            }
        }
    }
}
