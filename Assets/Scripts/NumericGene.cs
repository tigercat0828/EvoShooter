public class NumericGene {
    public int HealthPoint;
    public int AttackPoint;
    public float FireRate;
    public int MagazineSize;
    public float ReloadTime;
    public float BulletSpeed;
    public float MoveSpeed;
    public float RotateSpeed;
    public float ViewDistance;

    public static NumericGene DefaultGene;
    static NumericGene() {
        GameSettings.LoadSettings();
        DefaultGene.HealthPoint = GameSettings.options.Agent_HealthPoint;
        DefaultGene.AttackPoint = GameSettings.options.Agent_AttackPoint;
        DefaultGene.FireRate = GameSettings.options.Agent_FireRate;
        DefaultGene.MagazineSize = GameSettings.options.Agent_MagazineSize;
        DefaultGene.ReloadTime = GameSettings.options.Agent_ReloadTime;
        DefaultGene.BulletSpeed = GameSettings.options.Agent_BulletSpeed;
        DefaultGene.MoveSpeed = GameSettings.options.Agent_MoveSpeed;
        DefaultGene.RotateSpeed = GameSettings.options.Agent_RotateSpeed;
        DefaultGene.ViewDistance = GameSettings.options.Agent_ViewDistance;
    }

    public NumericGene(int healthPoint, int attackPoint, float fireRate, int magazineSize, float reloadTime, float moveSpeed, float rotateSpeed, float viewDistance) {
        HealthPoint = healthPoint;
        AttackPoint = attackPoint;
        FireRate = fireRate;
        MagazineSize = magazineSize;
        ReloadTime = reloadTime;
        MoveSpeed = moveSpeed;
        RotateSpeed = rotateSpeed;
        ViewDistance = viewDistance;
    }
}
