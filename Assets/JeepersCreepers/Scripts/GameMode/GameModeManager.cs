using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine.Events;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    // when player kills, bind event to update the players teams kills
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
                    // win condition
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
                    // win condition
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
                sync.ResetScores();
                // reset score
                // reset player positions?
                // disable movement / shooting ?
                yield return new WaitForSeconds(2);
                // enable movement / shooting
                redWin.SetActive(false);
                break;

            case (int)Team.blue:
                blueWin.SetActive(true);
                yield return new WaitForSeconds(2);
                blueWin.SetActive(false);
                break;
        }
    }
}