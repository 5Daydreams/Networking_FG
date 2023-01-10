using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine.UI;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    // teams show up when you join
    // when you pick a team the ui disappears

    // get a reference of the spawned player ?
    // set team and change color ?
    // remove when the player leaves and open up a slot
    // array for the teams up to 5v5
    
    // 5 slots for players to join 
    // 0 if they're open 1 if they're closed
    private int[] redTeam = { 0, 0, 0, 0, 0 };
    private int[] blueTeam = { 0, 0, 0, 0, 0 };

    private int playerTeam = -1;
    private int playerID = 0;

    private ushort indx;

    [SerializeField]
    private Button onJoinTeamRedButton;
    [SerializeField]
    private Button onJoinTeamBlueButton;

    public Material redMaterial;
    public Material blueMaterial;

    Alteruna.Avatar avatar;
    [SerializeField]
    private GameObject player;

    private enum Team
    {
        red = 0,
        blue = 1
    };

    private Multiplayer _aump;

    void Start()
    {
        avatar = GetComponent<Alteruna.Avatar>();

        // Multiplayer setup
        if (_aump != null)
        {
            _aump.Disconnected.AddListener(HandleDisconnect);
            _aump.RoomLeft.AddListener(HandleRoomLeft);
        }

        // Button setup
        onJoinTeamRedButton.onClick.AddListener(() => { JoinTeam((int)Team.red); });
        onJoinTeamBlueButton.onClick.AddListener(() => { JoinTeam((int)Team.blue); });
        
    }

    public void HandleDisconnect(Multiplayer multiplayer, Endpoint endPoint)
    {
        LeaveTeam();
    }

    public void HandleRoomLeft(Multiplayer multiplayer) 
    {
        LeaveTeam();
    }

    private void LeaveTeam()
    {
        // check which team you're a part of
        // remove a slot from the array for that team

    }

    public void JoinTeam(int team)
    {
        switch (team)
        {
            case (int)Team.red:
                AssignTeam(redTeam);
                break;
            case (int)Team.blue:
                AssignTeam(blueTeam);
                break;
        }
    }

    void AssignTeam(int[] team)
    {
        if (avatar.IsMe)
        {
            var mesh = avatar.GetComponent<MeshRenderer>();
            mesh.material = redMaterial;

        }

        //for (int i = 0; i < team.Length; i++)
        //{
        //    if (team[i] == 1) { return; }
        //    // give color
        //    // give team id?
        //
        //}
    }
}
