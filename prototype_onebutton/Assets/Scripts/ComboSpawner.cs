using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSpawner : MonoBehaviour
{
    [SerializeField] private GameObject comboSpawner;
    [SerializeField] private ComboSystem comboSystem;
    [SerializeField] private List<GameObject> toSpawn;

    private bool isSpawning = false;
    private float spawnTimer = 0.0f;
    private int spawnIndex = 0;
    
    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (isSpawning && spawnTimer <= 0.0f)
        {
            Instantiate(toSpawn[spawnIndex], comboSpawner.transform);
            spawnIndex++;
            spawnTimer = Random.Range(0.3f, 1.5f);

            if (spawnIndex >= toSpawn.Count)
            {
                spawnIndex = 0;
            }
        }
    }

    public bool IsSpawning()
    {
        return isSpawning;
    }

    public void StartSpawning()
    {
        isSpawning = true;
        spawnIndex = 0;
    }

    public void StopSpawning()
    {
        isSpawning = false;
        spawnIndex = 0;
    }
}
