using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class GameModeSync : AttributesSync
{
    [SynchronizableField] [SerializeField] private int redTeamScore;
    [SynchronizableField] [SerializeField] private int blueTeamScore;

    [SerializeField] private GameModeUI UI;

    public int GetRedTeamScore() { return redTeamScore; }
    public int GetBlueTeamScore() { return blueTeamScore; }

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
        UI.UpdateRedScore(0);
    }
}