using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealtUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healtText;
    private PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (true)
        {

        }
        healtText.text = playerHealth.health.ToString();
    }
}
