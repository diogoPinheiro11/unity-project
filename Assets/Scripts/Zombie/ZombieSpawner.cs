using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // Prefab do Zombie
    public int limiteZombies = 30; // Número total de zombies a spawnar
    public float spawnInterval = 10f; // Intervalo de spawn em segundos
    public int zombiesPerSpawn = 2; // Quantidade de zombies por spawn

    private int totalZombies = 0; // Contador de zombies spawnados

    void Start()
    {
        StartCoroutine(SpawnZombiesRoutine());
    }

    IEnumerator SpawnZombiesRoutine()
    {
        while (totalZombies < limiteZombies)
        {
            yield return new WaitForSeconds(spawnInterval);

            for (int i = 0; i < zombiesPerSpawn; i++)
            {
                SpawnZombie();
            }
        }
    }

    void SpawnZombie()
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * transform.localScale.x;
        randomPosition.y = 0.8f; // Define a altura zero para evitar posições no ar

        Instantiate(zombiePrefab, randomPosition, Quaternion.identity);
        totalZombies++;
    }
}