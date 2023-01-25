using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine.Events;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    // when player kills, bind event to update the players teams kills
    [SerializeField] private GameModeSync sync;
    [SerializeField] private int winScore = 29;

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
                }
                else
                {
                    sync.HandleRedTeamScore();
                }
                break;

            case (int)Team.blue:
                int blueScore = sync.GetBlueTeamScore();
                if (blueScore >= winScore)
                {
                    // win condition
                    sync.HandleBlueTeamScore();
                }
                else
                {
                    sync.HandleBlueTeamScore();
                }
                break;
        }
    }
}