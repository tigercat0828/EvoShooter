using System;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;

[Serializable]
public class Gene {
    private const int MAX_MUTATE_SWAP_TIME = 5;
    private const int MAX_MUTATE_AMOUNT = 10;
    private const int MAX_SKILL_POINT = 100;
    private const int GENE_LENGTH = 9;
    public int[] frags = new int[GENE_LENGTH];
   
    public int HealthPoint      => frags[0];
    public int AttackPoint      => frags[1];
    public int FireRate         => frags[2];
    public int MagazineSize     => frags[3]; 
    public int ReloadTime       => frags[4]; 
    public int BulletSpeed      => frags[5];
    public int MoveSpeed        => frags[6];
    public int RotateSpeed      => frags[7];
    public int ViewDistance     => frags[8];
    public Gene(int[] stats) {
        Array.Copy(stats, this.frags, GENE_LENGTH);
    }
    public Gene(Gene other) {
        Array.Copy(other.frags, frags, GENE_LENGTH);
    }
    public int this[int index] {
        get => frags[index];
        set => frags[index] = value;
    }
    public override string ToString() {
        return $"{HealthPoint},{AttackPoint},{FireRate},{MagazineSize},{ReloadTime},{BulletSpeed},{MoveSpeed}, {RotateSpeed},{ViewDistance}";
    }
    public static Gene GenRandomGene() {
        
        int[] stats = new int[9];
        for (int i = 0; i < stats.Length; i++) {
            stats[i] = UnityEngine.Random.Range(1, MAX_SKILL_POINT+1);
        }
        return Normalize(stats);
    }
    public static (Gene, Gene) CrossOver(Gene a, Gene b) {
        
        for (int i = 0; i < MAX_MUTATE_SWAP_TIME; i++) {
            int t = UnityEngine.Random.Range(0, GENE_LENGTH);
            (a[t], b[t]) = (b[t], a[t]);
        }
        Gene A = Normalize(a.frags);
        Gene B = Normalize(b.frags);
        return (A, B);
    }
    public static Gene Normalize(int[] stats) {

        int total = stats.Sum();
        double scaleFactor = 100.0 / total;
        stats = stats.Select(s => (int)(s * scaleFactor)).ToArray();
        total = stats.Sum();

        // If the sum is not exactly 100 due to rounding, adjust the random element
        if (total != 100) {
            int randomIndex = UnityEngine.Random.Range(0,stats.Length);
            stats[randomIndex] += 100 - total;
        }
        return new Gene(stats);
    }
    public static Gene Mutate(Gene gene) {
         int[] newStats = new int[9];
        Array.Copy(gene.frags, newStats, GENE_LENGTH);
        int i = UnityEngine.Random.Range(0, GENE_LENGTH);
        int j = UnityEngine.Random.Range(0, GENE_LENGTH);
        int amount = UnityEngine.Random.Range(0, MAX_MUTATE_AMOUNT); // 0~10
        if (newStats[i] < amount) {
            newStats[j] += newStats[i];
            newStats[i] = 0;
        }
        else {
            newStats[i] -= amount;
            newStats[j] += amount;
        }
        return Normalize(newStats);
       
    }
}
