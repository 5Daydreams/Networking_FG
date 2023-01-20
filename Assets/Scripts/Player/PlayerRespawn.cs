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

    public IEnumerator Respawn(Alteruna.Avatar avatar, float spawnTime)
    {
        RigidbodySynchronizable rb = avatar.GetComponent<RigidbodySynchronizable>();
        Debug.Log("STARTING RESPAWN ROUTINE");
        yield return new WaitForSeconds(spawnTime);
        rb.position = SpawnPossitions[0].transform.position;
    }
}
