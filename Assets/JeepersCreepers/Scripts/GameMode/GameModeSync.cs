using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class GameModeSync : AttributesSync
{
    [SynchronizableField] [SerializeField] private int redTeamScore;
    [SynchronizableField] [SerializeField] private int blueTeamScore;
    [SerializeField] private GameObject redWin;
    [SerializeField] private GameObject blueWin;
    [SerializeField] private GameModeUI UI;

    public int GetRedTeamScore() { return redTeamScore; }
    public int GetBlueTeamScore() { return blueTeamScore; }

    private bool bRedUIIsActive = false;
    private bool bBlueUIIsActive = false;
    public void HandleRedTeamScore()
    {
        redTeamScore++;
        BroadcastRemoteMethod("HandleUIChangeRed");
    }

    public void HandleBlueTeamScore()
    {
        blueTeamScore++;
        BroadcastRemoteMethod("HandleUIChangeBlue");
    }

    [SynchronizableMethod]
    public void HandleUIChangeRed()
    {
        UI.UpdateRedScore(redTeamScore);
    }

    [SynchronizableMethod]
    public void HandleUIChangeBlue()
    {
        UI.UpdateBlueScore(blueTeamScore);
    }

    public void ResetScores()
    {
        redTeamScore = 0;
        blueTeamScore = 0;
        BroadcastRemoteMethod("HandleUIReset");
    }

    [SynchronizableMethod]
    public void HandleUIReset()
    {
        UI.UpdateRedScore(0);
        UI.UpdateBlueScore(0);
    }

    public void HandleWinScreen(int team)
    {
        switch (team)
        {
            case (int)Team.red:
                BroadcastRemoteMethod("ToggleRedWinUI");
                break;

            case (int)Team.blue:
                BroadcastRemoteMethod("ToggleBlueWinUI");
                break;
        }
    }

    [SynchronizableMethod]
    public void ToggleRedWinUI()
    {
        bRedUIIsActive = !bRedUIIsActive;
        redWin.SetActive(bRedUIIsActive);
    }

    [SynchronizableMethod]
    public void ToggleBlueWinUI()
    {
        bBlueUIIsActive = !bBlueUIIsActive;
        blueWin.SetActive(bBlueUIIsActive);
    }
}