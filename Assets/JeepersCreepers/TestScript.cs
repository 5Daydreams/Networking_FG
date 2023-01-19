using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Alteruna;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private SyncTesting sync;
    [SerializeField] private Button joinRedTeam;
    [SerializeField] private Button joinBlueTeam;

    private int currentTeam;

    private Multiplayer _aump;
    
    enum Team 
    { 
        red = 0,
        blue = 1
    }

    private void Start()
    {
        currentTeam = -1;

        StartCoroutine(SetUpUI());

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
            _aump.OtherUserJoined.AddListener(HandleJoined);
        }

        // Button Setup
        joinRedTeam.onClick.AddListener(() => { JoinTeam((int)Team.red); });
        joinBlueTeam.onClick.AddListener(() => { JoinTeam((int)Team.blue); });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            sync.AssignTeam((int)Team.red);
            sync.UpdateTeamSize();
            sync.UpdateTeamManager();
            currentTeam = (int)Team.red;

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            sync.AssignTeam((int)Team.blue);
            sync.UpdateTeamSize();
            sync.UpdateTeamManager();
            currentTeam = (int)Team.blue;

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            sync.UpdateTeamSize();
            CanJoin();
        }
    }

    public void HandleJoined(Multiplayer multiplayer, User user)
    {
        StartCoroutine(UpdateNewPlayer());
    }

    IEnumerator SetUpUI()
    {
        yield return new WaitForSeconds(1f);
        sync.UpdateTeamSize();
        CanJoin();
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

        joinRedTeam.gameObject.SetActive(false);
        joinBlueTeam.gameObject.SetActive(false);
    }

    private void CanJoin()
    {
        int redTeamSize = sync.GetRedTeamSize();
        int blueTeamSize = sync.GetBlueTeamSize();

        // Both teams are full
        if (redTeamSize == blueTeamSize && redTeamSize == 5) 
        {
            joinRedTeam.gameObject.SetActive(false);
            joinBlueTeam.gameObject.SetActive(false);
            Debug.Log("Team is full");
        } 
        // Join blue team
        if (redTeamSize > blueTeamSize) 
        { 
            joinRedTeam.gameObject.SetActive(false);
            joinBlueTeam.gameObject.SetActive(true);
            Debug.Log("Blue team UI");
        }
        // Join red team
        if (redTeamSize < blueTeamSize) 
        {
            joinRedTeam.gameObject.SetActive(true);
            joinBlueTeam.gameObject.SetActive(false);
            Debug.Log("Red team UI");
        }
        // Join any team
        if (redTeamSize == blueTeamSize && redTeamSize != 5) 
        {
            joinRedTeam.gameObject.SetActive(true);
            joinBlueTeam.gameObject.SetActive(true);
            Debug.Log("Both UI");
        } 
    }
}