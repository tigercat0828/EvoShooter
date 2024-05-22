public class Gene {
    public int HealthPoint;
    public int AttackPoint;
    public float FireRate;
    public int MagazineSize;
    public float MoveSpeed;
    public float RotateSpeed;
    public float ViewDistance;

    public Gene(int healthPoint, int attackPoint, float fireRate, int magazineSize, float moveSpeed, float rotateSpeed, float viewDistance) {
        HealthPoint = healthPoint;
        AttackPoint = attackPoint;
        FireRate = fireRate;
        MagazineSize = magazineSize;
        MoveSpeed = moveSpeed;
        RotateSpeed = rotateSpeed;
        ViewDistance = viewDistance;
    }
}
