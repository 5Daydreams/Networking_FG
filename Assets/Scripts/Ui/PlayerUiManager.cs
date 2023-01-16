using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUiManager : MonoBehaviour
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
        healthText.text = playerHealt.GetHealt().ToString();

        killsText.text = "K:" + playerKDA.kills.ToString();
        deathsText.text = "D:" + playerKDA.deaths.ToString();
        assistText.text = "A:" + playerKDA.assist.ToString();
    }
}
