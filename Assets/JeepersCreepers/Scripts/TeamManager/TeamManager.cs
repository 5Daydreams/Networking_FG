using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine.UI;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField] private TeamManagerSync sync;
    [SerializeField] private TeamManagerUI UI;
    [SerializeField] private Button joinRedTeam;
    [SerializeField] private Button joinBlueTeam;

    [SerializeField] private Alteruna.Avatar avatar;
    [SerializeField] private RocketLauncherGun gun;
    private int currentTeam;

    private Multiplayer multiplayer;

    private void Start()
    {
        UI.DisableUI();

        if(avatar.IsMe)
        {
            UI.EnableUI();
            StartCoroutine(SetUpUI());
        }

        currentTeam = -1;

        if (multiplayer == null)
        {
            multiplayer = FindObjectOfType<Multiplayer>();
            if (multiplayer == null)
            {
                Debug.LogError("Unable to find a active object of type Multiplayer.");
            }
        }
        // Multiplayer setup
        if (multiplayer)
        {
            multiplayer.OtherUserJoined.AddListener(HandleJoined);
            multiplayer.OtherUserLeft.AddListener(OtherPlayerLeft); 
        }

        // Button Setup
        joinRedTeam.onClick.AddListener(() => { JoinTeam((int)Team.red); });
        joinBlueTeam.onClick.AddListener(() => { JoinTeam((int)Team.blue); });
    }

    public void HandleJoined(Multiplayer multiplayer, User user)
    {
        StartCoroutine(UpdateNewPlayer());
    }

    IEnumerator SetUpUI()
    {
        yield return new WaitForSeconds(1f);
        sync.UpdateTeamSize();

        int redTeamSize = sync.GetRedTeamSize();
        int blueTeamSize = sync.GetBlueTeamSize();

        UI.UpdateTeamUI(redTeamSize, blueTeamSize);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator UpdateNewPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        sync.AssignTeam(currentTeam);
        sync.UpdateTeamSize();
        sync.UpdateTeamManager();
    }

    public void JoinTeam(int team)
    {
        switch (team)
        {
            case (int)Team.red:
                sync.AssignTeam((int)Team.red);
                sync.UpdateTeamSize();
                sync.UpdateTeamManager();
                currentTeam = (int)Team.red;
                break;

            case (int)Team.blue:
                sync.AssignTeam((int)Team.blue);
                sync.UpdateTeamSize();
                sync.UpdateTeamManager();
                currentTeam = (int)Team.blue;
                break;
        }

        UI.DisableUI();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gun.bJoinedTeam = true;

        joinRedTeam.gameObject.SetActive(false);
        joinBlueTeam.gameObject.SetActive(false);
    }

    IEnumerator UpdatePlayerLeft()
    {
        yield return new WaitForSeconds(0.5f);
        sync.UpdateTeamSize();
    }

    private void OtherPlayerLeft(Multiplayer multiplayer, User user)
    {
        StartCoroutine(UpdatePlayerLeft());
    }
}
