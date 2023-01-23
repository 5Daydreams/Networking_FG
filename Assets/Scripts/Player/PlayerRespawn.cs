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
        float closestPlayer = 0;
        rb = avatar.GetComponent<RigidbodySynchronizable>();
        MeshRenderer[] meshrenderers = avatar.GetComponentsInChildren<MeshRenderer>();
        avatar.GetComponent<CapsuleCollider>().enabled = false;

        for (int i = 0; i < meshrenderers.Length; i++)
        {
            meshrenderers[i].enabled = false;
        }

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

        yield return new WaitForSeconds(spawnTime);

        avatar.GetComponent<CapsuleCollider>().enabled = true;

        rb.position = spawnPosition;

        for (int i = 0; i < meshrenderers.Length; i++)
        {
            meshrenderers[i].enabled = true;
        }
    }
}
