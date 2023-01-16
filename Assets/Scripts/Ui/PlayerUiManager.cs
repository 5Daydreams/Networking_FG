using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Alteruna;

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
    [SerializeField] private GameObject playerStats;
    [SerializeField] private TextMeshProUGUI AvatarName;
    [SerializeField] private TextMeshProUGUI killScore;
    [SerializeField] private TextMeshProUGUI deathScore;
    [SerializeField] private TextMeshProUGUI assitScore;

    public List<GameObject> statList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (!avatar.IsMe)
            canvas.enabled = false;
        else
        {
            AvatarName.text = avatar.GetInstanceID().ToString();
            statList.Add(playerStats);
            BroadcastRemoteMethod("UpdateScoreboard");
        }
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
            leaderBoardUi.SetActive(true);
        if (Input.GetKeyUp(KeyCode.Tab))
            leaderBoardUi.SetActive(false);
    }

    public void UpdateHealthText()
    {
        healthText.text = playerHealt.GetHealt().ToString();
    }

    public void UpdateKDAText()
    {
        killScore.text = killsText.text = "K:" + playerKDA.kills.ToString();
        deathScore.text = deathsText.text = "D:" + playerKDA.deaths.ToString();
        assitScore.text = assistText.text = "A:" + playerKDA.assist.ToString();
    }

    [SynchronizableMethod]
    public void UpdateScoreboard()
    {
        for (int i = 0; i < statList.Count; i++)
        {
            playerStats.transform.position += Vector3.up * -200 * i;
        }
    }
}
