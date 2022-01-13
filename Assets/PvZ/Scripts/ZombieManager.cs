using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZombieManager : MonoBehaviour
{
    public GameObject[] zombies;

    [Header("Waves")]
    public int waveCount;
    public int difficultyMultiplier;
    public int zombiesPerWave;
    public float zombieSpawnSpeedMultiplier;
    public int seed;
    public float timeBetweenWaves;
    public float timeBetweenZombies;
    public float startDelay;

    int row;
    int col;
    bool waveActive = false;

    void Start()
    {
        //Random.InitState(seed);
        row = this.GetComponent<GenerateWorld>().row;
        col = this.GetComponent<GenerateWorld>().col;

        Invoke("StartWave", startDelay);
    }

    void Update()
    {
        //Debug.Log(timeBetweenZombies);
        if(GameObject.FindGameObjectsWithTag("Zombie").Length == 0 && waveActive)
        {
            waveActive = false;
            if (waveCount > 0)
            {
                Invoke("StartWave", timeBetweenWaves);
                Debug.Log("Start next wave --------------------------");
            }
            else
            {
                SceneManager.LoadScene("WinScene");
            }
            
            
        }
    }
    void StartWave()
    {
        waveActive = true;
        waveCount--;
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        for (int i = 0; i < zombiesPerWave; i++)
        {
            Vector3 spawnPos = new Vector3(col + 2, 0f, Random.Range(0, row));

            Commands.Instance.Spawn(zombies[0], spawnPos, new Vector3(0, -90, 0), "Zombie", "Zombie", null);

            timeBetweenZombies = timeBetweenZombies / zombieSpawnSpeedMultiplier;
            yield return new WaitForSeconds( timeBetweenZombies);
        }  
    }

}
