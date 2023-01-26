using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine.Events;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] private GameModeSync sync;
    [SerializeField] private int winScore;
    [SerializeField] private GameObject redWin;
    [SerializeField] private GameObject blueWin;

    public void UpdateTeamKills(int team)
    {
        switch (team)
        {
            case (int)Team.red:
                int redScore = sync.GetRedTeamScore();
                if (redScore >= winScore)
                {
                    sync.HandleRedTeamScore();
                    StartCoroutine(WinCondition((int)Team.red));
                }
                else
                {
                    sync.HandleRedTeamScore();
                }
                break;

            case 1:
                int blueScore = sync.GetBlueTeamScore();
                if (blueScore >= winScore)
                {
                    sync.HandleBlueTeamScore();
                    StartCoroutine(WinCondition((int)Team.blue));
                }
                else
                {
                    sync.HandleBlueTeamScore();
                }
                break;
        }
    }

    IEnumerator WinCondition(int team)
    {
        switch (team)
        {
            case (int)Team.red:
                redWin.SetActive(true);
                sync.HandleWinScreen((int)Team.red);

                sync.ResetScores();

                yield return new WaitForSeconds(2);

                redWin.SetActive(false);
                sync.HandleWinScreen((int)Team.red);
                break;

            case (int)Team.blue:
                blueWin.SetActive(true);
                sync.HandleWinScreen((int)Team.blue);

                sync.ResetScores();

                yield return new WaitForSeconds(2);

                blueWin.SetActive(false);
                sync.HandleWinScreen((int)Team.blue);
                break;
        }
    }
}