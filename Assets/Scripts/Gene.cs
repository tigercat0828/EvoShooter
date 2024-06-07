using System;
using System.Linq;

[System.Serializable]
public class Gene {
    private const int SWAP_NUM = 5;
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
        HealthPoint         = healthPoint;
        AttackPoint         = attackPoint;
        FireRate            = fireRate;
        MagazineSize        = magazineSize;
        ReloadTime          = reloadTime;
        BulletSpeed         = bulletSpeed;
        MoveSpeed           = moveSpeed;
        RotateSpeed         = rotateSpeed;
        ViewDistance        = viewDistance;
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
    public int this[int index] {
        get {
            switch (index) {
                case 0: return HealthPoint;
                case 1: return AttackPoint;
                case 2: return FireRate;
                case 3: return MagazineSize;
                case 4: return ReloadTime;
                case 5: return BulletSpeed;
                case 6: return MoveSpeed;
                case 7: return RotateSpeed;
                case 8: return ViewDistance;
                default: throw new IndexOutOfRangeException("Invalid index");
            }
        }
        set {
            switch (index) {
                case 0: HealthPoint = value; break;
                case 1: AttackPoint = value; break;
                case 2: FireRate = value; break;
                case 3: MagazineSize = value; break;
                case 4: ReloadTime = value; break;
                case 5: BulletSpeed = value; break;
                case 6: MoveSpeed = value; break;
                case 7: RotateSpeed = value; break;
                case 8: ViewDistance = value; break;
                default: throw new IndexOutOfRangeException("Invalid index");
            }
        }
    }
    public override string ToString() {
        return $"{HealthPoint},{AttackPoint},{FireRate},{MagazineSize},{ReloadTime},{BulletSpeed},{MoveSpeed}, {RotateSpeed},{ViewDistance}";
    }
    public static Gene GenRandomGene() {
        
        int[] stats = new int[9];
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = UnityEngine.Random.Range(1, 101);
        }
        
        return Normalize(stats);
    }
    public static (Gene, Gene) CrossOver(Gene a, Gene b) {
        int[] statsA = new int[9];
        int[] statsB = new int[9];
        for (int i = 0; i < 9; i++) {
            statsA[i] = a[i];
            statsB[i] = b[i];
        }
        int[] idx = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        for (int i = idx.Length - 1; i > 0; i--) {
            int j = UnityEngine.Random.Range(0, i + 1);
            (idx[j], idx[i]) = (idx[i], idx[j]);
        }
        idx = idx.Take(5).ToArray();
        for (int i = 0; i < SWAP_NUM; i++) {
            (statsA[idx[i]], statsB[idx[i]]) = (statsB[idx[i]], statsA[idx[i]]);
        }
        Gene A = Normalize(statsA);
        Gene B = Normalize(statsB);
        return (A, B);
    }
    public static Gene Normalize(int[] stats) {

        int total = stats.Sum();
        // Normalize the values
        
        double scaleFactor = 100.0 / total;
        stats = stats.Select(s => (int)(s * scaleFactor)).ToArray();
        total = stats.Sum();

        // If the sum is not exactly 100 due to rounding, adjust the random element
        if (total != 100) {
            int randomIndex = UnityEngine.Random.Range(0,stats.Length);
            stats[randomIndex] += 100 - total;
        }
        return new Gene(stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], stats[6], stats[7], stats[8]);
    }
    public static Gene Mutate(Gene gene) {
        int[] stats = new int[9];
        stats[0] = gene.HealthPoint;
        stats[1] = gene.AttackPoint;
        stats[2] = gene.FireRate;
        stats[3] = gene.MagazineSize;
        stats[4] = gene.ReloadTime;
        stats[5] = gene.BulletSpeed;
        stats[6] = gene.MoveSpeed;
        stats[7] = gene.RotateSpeed;
        stats[8] = gene.ViewDistance;
        int i = UnityEngine.Random.Range(0, 8);
        int j = UnityEngine.Random.Range(0, 8);
        int amount = UnityEngine.Random.Range(0, 10);
        if (stats[i] < amount) {
            stats[j] += stats[i];
            stats[i] = 0;
        }
        else {
            stats[i] -= amount;
            stats[j] += amount;
        }
        return Normalize(stats);
    }
}
