using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private float SpawnTime = 1f;
    [SerializeField] private GameObject[] enemyPrefab;
    private float spawnTimer = 0;

    private void Update() {
        if (spawnTimer > SpawnTime) {
            spawnTimer = 0;
            int random = Random.Range(0, enemyPrefab.Length);
            GameObject enemyToSpawn = enemyPrefab[random];
            Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        }
        spawnTimer += Time.deltaTime;
    }


}
