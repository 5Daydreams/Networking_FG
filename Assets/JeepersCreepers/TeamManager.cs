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
    //[SynchronizableField]
    private int redTeam = 0;
    //[SynchronizableField]
    private int blueTeam = 0;

    private int playerTeam = -1;
    private int playerID = 0;

    bool bSetColor = false;

    [SerializeField]
    private Button onJoinTeamRedButton;
    [SerializeField]
    private Button onJoinTeamBlueButton;

    public Material redMaterial;
    public Material blueMaterial;

    Alteruna.Avatar[] avatars;
    Alteruna.Avatar[] tempAvatars;

    public Alteruna.Avatar avatar;
    public MeshRenderer renderer;
    MeshRenderer[] epic = new MeshRenderer[1];

    private enum Team
    {
        red = 0,
        blue = 1
    };

    private Multiplayer _aump;

    void Start()
    {
        if (_aump == null)
        {
            _aump = FindObjectOfType<Multiplayer>();
            if (!_aump)
            {
                Debug.LogError("Unable to find a active object of type Multiplayer.");
            }
        }
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

    private void Update()
    {
        // check teamsizes
        // if one team is greater than the other
        // hide button
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(_aump != null && avatar.IsMe)
            {
                UniqueAvatarColor color = new UniqueAvatarColor();
                epic[0] = renderer;
                color.meshes = epic;
                renderer.material = blueMaterial;
                color.UpdateHue();
            }
        }
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
                AssignTeam(team);
                // Set team color  
                break;

            case (int)Team.blue:
                AssignTeam(team);
                break;
        }
    }

    //[SynchronizableMethod]
    void AssignTeam(int team)
    {
        if (avatar)
        {
            // Assign the team
            switch (team)
            {
                // Assign to the red team
                case (int)Team.red:
                    // Set team ID
                    
                   //user.team = team;
                   //user.SetMaterial(redMaterial);

                    // increment team size
                    redTeam++;
                    break;

                // Assign to the blue team
                case (int)Team.blue:
                    //user.team = team;
                    //user.SetMaterial(blueMaterial);
                    blueTeam++;
                    break;
            }
        }
    }
}
