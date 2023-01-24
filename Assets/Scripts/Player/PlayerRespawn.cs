using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Alteruna;

public class PlayerRespawn : MonoBehaviour
{
    public int reSpawnTimer = 1;
    public GameObject[] SpawnPossitions;

    TextMeshProUGUI timerText;

    Vector3 spawnPosition;
    float distanceToPlayer;

    AvatarCollection avatarCollection;
    
    private void Awake()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();
    }

    public void Respawn(RigidbodySynchronizable rb)
    {
        float closestPlayer = 0;

        for (int i = 0; i < SpawnPossitions.Length; i++)
        {
            foreach (var player in avatarCollection.avatars)
            {
                distanceToPlayer = Vector3.Distance(SpawnPossitions[i].transform.position, player.Value.transform.position);

                if (closestPlayer < distanceToPlayer || closestPlayer == 0)
                {
                    closestPlayer = distanceToPlayer;
                    spawnPosition = SpawnPossitions[i].transform.position;
                }
            }
        }

        rb.position = spawnPosition;
    }
}
