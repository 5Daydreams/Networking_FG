using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class TeamManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject buttonCanvas;
    [SerializeField] private Button redTeamButton;
    [SerializeField] private Button blueTeamButton;

    [SerializeField] private TextMeshProUGUI redTeamSizeText;
    [SerializeField] private TextMeshProUGUI blueTeamSizeText;

    private string outOfFive = " / 5";

    public void UpdateTeamUI(int redTeamSize, int blueTeamSize)
    {
        UpdateTeamSizes(redTeamSize, blueTeamSize);
        UpdateButtons(redTeamSize, blueTeamSize);
    }

    private void UpdateTeamSizes(int redTeamSize, int blueTeamSize)
    {
        redTeamSizeText.text = redTeamSize.ToString() + outOfFive;
        blueTeamSizeText.text = blueTeamSize.ToString() + outOfFive;
    }

    private void UpdateButtons(int redTeamSize, int blueTeamSize)
    {
        // which button to show
        // Both teams are full
        if (redTeamSize == blueTeamSize && redTeamSize == 5)
        {
            redTeamButton.gameObject.SetActive(false);
            blueTeamButton.gameObject.SetActive(false);
        }
        // Join blue team
        if (redTeamSize > blueTeamSize)
        {
            redTeamButton.gameObject.SetActive(false);
            blueTeamButton.gameObject.SetActive(true);
        }
        // Join red team
        if (redTeamSize < blueTeamSize)
        {
            redTeamButton.gameObject.SetActive(true);
            blueTeamButton.gameObject.SetActive(false);
        }
        // Join any team
        if (redTeamSize == blueTeamSize && redTeamSize != 5)
        {
            redTeamButton.gameObject.SetActive(true);
            blueTeamButton.gameObject.SetActive(true);
        }
    }

    public void DisableUI()
    {
        buttonCanvas.gameObject.SetActive(false);
    }

    public void EnableUI()
    {
        buttonCanvas.gameObject.SetActive(true);
    }
}