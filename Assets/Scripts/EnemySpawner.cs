using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private bool canSpawn = true;
    private void Start() {
        StartCoroutine(Spawner());

    }

    // Fix : change to timer pattern
    public IEnumerator Spawner() {
        WaitForSeconds wait = new (spawnRate);
        while (true) {
            yield return wait;
            int random = Random.Range(0, enemyPrefab.Length);
            GameObject enemyToSpawn = enemyPrefab[random];
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
    }

}
