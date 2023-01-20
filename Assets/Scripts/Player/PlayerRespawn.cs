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
    RigidbodySynchronizable rb;
    
    private void Awake()
    {
        avatarCollection = FindObjectOfType<AvatarCollection>();
    }

    public IEnumerator Respawn(Alteruna.Avatar avatar, float spawnTime)
    {
        float distance = 0;
        rb = avatar.GetComponent<RigidbodySynchronizable>();

        foreach (GameObject spawn in SpawnPossitions)
        {
            int i = 0;
            foreach (var player in avatarCollection.avatars)
            {
                distanceToPlayer = Vector3.Distance(spawn.transform.position, player.Value.transform.position);

                if (distance > distanceToPlayer || distance == 0)
                {
                    distance = distanceToPlayer;
                    Debug.Log(SpawnPossitions[i]);
                    spawnPosition = SpawnPossitions[i].transform.position;
                }
                i++;
            }
        }


        yield return new WaitForSeconds(spawnTime);
        rb.position = spawnPosition;
    }
}
