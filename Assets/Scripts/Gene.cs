using UnityEngine;

[System.Serializable]
public class Gene {
    public int HealthPoint;
    public int AttackPoint;
    public float FireRate;
    public int MagazineSize;
    public float ReloadTime;
    public float BulletSpeed;
    public float MoveSpeed;
    public float RotateSpeed;
    public float ViewDistance;

    public Gene(int healthPoint, int attackPoint, float fireRate, int magazineSize, float reloadTime, float bulletSpeed,float moveSpeed, float rotateSpeed, float viewDistance) {
        HealthPoint = healthPoint;
        AttackPoint = attackPoint;
        FireRate = fireRate;
        MagazineSize = magazineSize;
        ReloadTime = reloadTime;
        BulletSpeed = bulletSpeed;
        MoveSpeed = moveSpeed;
        RotateSpeed = rotateSpeed;
        ViewDistance = viewDistance;
    }

    public static Gene GenRandomGene() {

        int[] stats = new int[9];
        int total = 0;

        // Generate random values and calculate their total
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = Random.Range(1, 101);
            total += stats[i];
        }
        // Normalize the values
        double scaleFactor = 100.0 / total;
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = (int)(stats[i] * scaleFactor);
        }
        total = 0;
        for (int i = 0; i < stats.Length; i++) {
            total += stats[i];
        }
        // If the sum is not exactly 100 due to rounding, adjust the last element
        if (total != 100) {
            stats[^1] += 100 - total;
        }
        return new Gene(
             stats[0] ,
             stats[1] ,
             stats[2] ,
             stats[3] ,
             stats[4] ,
             stats[5] ,
             stats[6] ,
             stats[7] ,
             stats[8] 
            );

    }
}
