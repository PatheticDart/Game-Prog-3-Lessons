using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject giantMonsterPrefab;
    public GameObject crawlerMonsterPrefab;
    public GameObject mutantMonsterPrefab;
    
    public Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SpawnEnemy("Giant"); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SpawnEnemy("Crawler");}
        if (Input.GetKeyDown(KeyCode.Alpha3)) { SpawnEnemy("Mutant");}
    }

    void SpawnEnemy(string Type)
    {
        GameObject prefabToSpawn = null;

        switch (Type)
        {
            case "Giant":
                prefabToSpawn = giantMonsterPrefab;
                Debug.Log("Giant Monster");
                break;
            case "Crawler":
                prefabToSpawn = crawlerMonsterPrefab;
                Debug.Log("Crawler Monster");
                break;
            case "Mutant":
                prefabToSpawn = mutantMonsterPrefab;
                Debug.Log("Mutant Monster");
                break;
            default:
                Debug.Log("No Monster");
                break;
        }

        if (prefabToSpawn == null)
        {
            return;
        }
        else
        {
            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
