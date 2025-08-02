using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance = null;
    public SpawnPoint playerSpawnPoint;
    public Player player;

    void Awake()
    {
        if (sharedInstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    void Start()
    {
        SetupSence();
    }

    public void SetupSence()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        if (playerSpawnPoint != null)
        {
            GameObject playerObject = playerSpawnPoint.SpawnObject();
            player = playerObject.GetComponent<Player>();
        }
    }
}
