using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager sharedInstance = null;
    public SpawnPoint playerSpawnPoint;
    public Player player;

    public GameObject enemyAmmoPrefab;
    public int poolSize = 20;
    public Queue<GameObject> ammoPool = new Queue<GameObject>();
    
    
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

    private void InitializeAmmoPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject ammo = Instantiate(enemyAmmoPrefab);
            ammo.SetActive(false);
            ammoPool.Enqueue(ammo);
        }
    }

    public GameObject GetAmmo()
    {
        if (ammoPool.Count > 0)
        {
            GameObject ammo = ammoPool.Dequeue();
            ammo.SetActive(true);
            return (ammo);
        }
        else
        {
            GameObject ammo = Instantiate(enemyAmmoPrefab);
            return (ammo);
        }

    }
    public void ReturnAmmo(GameObject ammo)
    {
        ammo.SetActive(false);
        ammoPool.Enqueue(ammo);
    }
}