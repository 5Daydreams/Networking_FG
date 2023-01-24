using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RespawnTimer : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] GameObject respawnTimer;
    [SerializeField] TextMeshProUGUI respawnText;

    private void Update()
    {
        if (playerHealth.dead)
        {
            respawnTimer.SetActive(true);
            playerHealth.spawnTimer -= Time.deltaTime;
            respawnText.text = "Respawning: " + playerHealth.spawnTimer.ToString("F1");
        }
        else
        {
            respawnTimer.SetActive(false);
        }
    }
}
