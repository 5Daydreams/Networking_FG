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


    // Start is called before the first frame update
    void Start()
    {
        if (!avatar.IsMe)
            canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ToggleScoreBoard();
        UpdateKDAText();
        UpdateHealthText();
    }

    public void UpdateHealthText()
    {
        healthText.text = playerHealt.GetHealth().ToString();
    }

    public void UpdateKDAText()
    {
        killsText.text = playerKDA.kills.ToString();
        deathsText.text = playerKDA.deaths.ToString();
        assistText.text = playerKDA.assist.ToString();
    }
}
