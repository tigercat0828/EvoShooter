using UnityEngine;

[System.Serializable]
public class Gene {
    public int HealthPoint;
    public int AttackPoint;
    public int FireRate;
    public int MagazineSize;
    public int ReloadTime;
    public int BulletSpeed;
    public int MoveSpeed;
    public int RotateSpeed;
    public int ViewDistance;

    public Gene(int healthPoint, int attackPoint, int fireRate, int magazineSize, int reloadTime, int bulletSpeed, int moveSpeed, int rotateSpeed, int viewDistance) {
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
    public Gene(Gene other) {
        HealthPoint = other.HealthPoint;
        AttackPoint = other.AttackPoint;
        FireRate = other.FireRate;
        MagazineSize = other.MagazineSize;
        ReloadTime = other.ReloadTime;
        BulletSpeed = other.BulletSpeed;
        MoveSpeed = other.MoveSpeed;
        RotateSpeed = other.RotateSpeed;
        ViewDistance = other.ViewDistance;
    }
    public override string ToString() {
        return $"{HealthPoint},{AttackPoint},{FireRate},{MagazineSize},{ReloadTime},{BulletSpeed},{MoveSpeed}, {RotateSpeed},{ViewDistance}";
    }
    public static Gene GenRandomGene() {

        int[] stats = new int[9];
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = Random.Range(1, 101);
        }
        return Normalize(stats);
    }
    public static (Gene, Gene) CrossOver(Gene a, Gene b) {
        int[] statsA = new int[9];
        int[] statsB = new int[9];
        statsA[0] = a.HealthPoint;
        statsA[1] = b.AttackPoint;
        statsA[2] = a.FireRate;
        statsA[3] = b.MagazineSize;
        statsA[4] = a.ReloadTime;
        statsA[5] = b.BulletSpeed;
        statsA[6] = a.MoveSpeed;
        statsA[7] = b.RotateSpeed;
        statsA[8] = a.ViewDistance;

        statsB[0] = b.HealthPoint;
        statsB[1] = a.AttackPoint;
        statsB[2] = b.FireRate;
        statsB[3] = a.MagazineSize;
        statsB[4] = b.ReloadTime;
        statsB[5] = a.BulletSpeed;
        statsB[6] = b.MoveSpeed;
        statsB[7] = a.RotateSpeed;
        statsB[8] = b.ViewDistance;

        Gene A = Normalize(statsA);
        Gene B = Normalize(statsB);
        return (A, B);
    }
    public static Gene Normalize(int[] stats) {

        int total = 0;
        // Generate random values and calculate their total
        for (int i = 0; i < stats.Length; i++) {
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
            stats[0],
            stats[1],
            stats[2],
            stats[3],
            stats[4],
            stats[5],
            stats[6],
            stats[7],
            stats[8]
           );
    }
    public static Gene Mutate (Gene gene) {
        int[] stats= new int[9];
        stats[0] =  gene.HealthPoint    ;
        stats[1] =  gene.AttackPoint    ;
        stats[2] =  gene.FireRate       ;
        stats[3] =  gene.MagazineSize   ;
        stats[4] =  gene.ReloadTime     ;
        stats[5] =  gene.BulletSpeed    ;
        stats[6] =  gene.MoveSpeed      ;
        stats[7] =  gene.RotateSpeed    ;
        stats[8] =  gene.ViewDistance   ;
        int i = Random.Range(0, 8);
        int j = Random.Range(0, 8);
        int amount = Random.Range(0, 10);
        stats[i] -= amount ; if (stats[i] < 0) stats[i] = 0;
        stats[j] += amount ;
        return Normalize(stats);
    }
}
