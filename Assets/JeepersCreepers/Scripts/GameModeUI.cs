using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Alteruna;
using TMPro;
using UnityEngine;

public class GameModeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI redTeamScoreText;
    [SerializeField] private TextMeshProUGUI blueTeamScoreText;

    public void UpdateRedScore(int score)
    {
        redTeamScoreText.text = score.ToString();
    }

    public void UpdateBlueScore(int score)
    {
        blueTeamScoreText.text = score.ToString();
    }
}
