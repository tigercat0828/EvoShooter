using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public int SlotNo = 0;
    [SerializeField] private float SpawnTime = 1f;
    [SerializeField] private GameObject[] enemyPrefab;
    private float spawnTimer = 0;
    int ratioZombie;
    int ratioSpitter;
    int ratioTank;
    int ratioCharger;

    int[] cumulativeRatios;
    private void Awake() {
        GameSettings.LoadSettings();
        ratioZombie = GameSettings.options.SpawnRate_Zombie;
        ratioSpitter = GameSettings.options.SpawnRate_Spitter;
        ratioCharger = GameSettings.options.SpawnRate_Charger;
        ratioTank = GameSettings.options.SpawnRate_Tank;
        CalculateSpawnRatios();
    }
    private void CalculateSpawnRatios() {
        cumulativeRatios = new int[enemyPrefab.Length];
        cumulativeRatios[0] = ratioZombie;
        if (enemyPrefab.Length > 1) cumulativeRatios[1] = cumulativeRatios[0] + ratioSpitter;
        if (enemyPrefab.Length > 2) cumulativeRatios[2] = cumulativeRatios[1] + ratioCharger;
        if (enemyPrefab.Length > 3) cumulativeRatios[3] = cumulativeRatios[2] + ratioTank;
    }

    private void SpawnEnemy() {
        int totalRatio = cumulativeRatios[cumulativeRatios.Length - 1];
        int randomValue = Random.Range(0, totalRatio);

        GameObject enemyToSpawn = null;

        for (int i = 0; i < cumulativeRatios.Length; i++) {
            if (randomValue < cumulativeRatios[i]) {
                enemyToSpawn = enemyPrefab[i];
                break;
            }
        }

        IEntity entity = Instantiate(enemyToSpawn, transform.position, Quaternion.identity).GetComponent<IEntity>();
        entity.SetSlot(SlotNo);
    }
    private void Start() {
        SpawnEnemy();
    }
    private void Update() {
        if (spawnTimer > SpawnTime) {
            spawnTimer = 0;
            SpawnEnemy();
        }
        spawnTimer += Time.deltaTime;
    }
}
