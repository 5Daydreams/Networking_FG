using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class SyncTesting : AttributesSync
{
    // Set Team
    [SynchronizableField] [SerializeField] public int teamID = -1;
    [SynchronizableField] [SerializeField] int redTeam = 0;
    [SynchronizableField] [SerializeField] int blueTeam = 0;

    private SyncTesting[] allPlayers;
    public Alteruna.Avatar avatar;
    public MeshRenderer renderer;

    enum Team
    {
        red = 0,
        blue = 1
    }

    public int GetRedTeamSize() { return redTeam; }
    public int GetBlueTeamSize() { return blueTeam; }

    public void UpdateTeamSize()
    {
        allPlayers = FindObjectsOfType<SyncTesting>();

        int tempRedTeam = 0;
        int tempBlueTeam = 0;

        foreach (var player in allPlayers)
        {
            switch (player.teamID)
            {
                case (int)Team.red:
                    tempRedTeam++;
                    break;

                case (int)Team.blue:
                    tempBlueTeam++;
                    break;
                    // No team (TeamID = -1)
                default:
                    break;
            }
        }

        redTeam = tempRedTeam;
        blueTeam = tempBlueTeam;
    }

    public void AssignTeam(int team)
    {
        if (avatar.IsMe == false) { return; }
        SetTeamID(team);
        SetColor(team);
    }

    public void SetTeamID(int team) 
    { 
        teamID = team;
    }

    public void SetColor(int team)
    {
        switch (team)
        {
            case (int)Team.red:
                renderer.material.color = Color.red;
                break;

            case (int)Team.blue:
                renderer.material.color = Color.blue;
                break;
        }
    }

    public void UpdateTeamManager() 
    {
        HandleColorChange();
    }

    public void HandleColorChange()
    {
        InvokeRemoteMethod("UpdateColors");
    }

    [SynchronizableMethod]
    public void UpdateColors()
    {
        allPlayers = FindObjectsOfType<SyncTesting>();

        foreach (var player in allPlayers)
        {
            switch (player.teamID) 
            { 
                case (int)Team.red:
                    player.renderer.material.color = Color.red;
                    break;

                case (int)Team.blue:
                    player.renderer.material.color = Color.blue;
                    break;
            }
        }
    }
}