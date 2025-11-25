using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : WeaponEffect
{
    public enum DamageSource { projectile, owner};
    public DamageSource damageSource = DamageSource.projectile;
    public bool hasAutoAim = false;
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);

    protected Rigidbody2D rb;
    protected int piercing;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Weapon.Stats stats = weapon.GetStats();
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.linearVelocity = transform.right * stats.speed;
        }

        float area = stats.area == 0 ? 1 : stats.area;
        transform.localScale = new Vector3(
            area * Mathf.Sign(transform.localScale.x),
            area * Mathf.Sign(transform.localScale.y), 1
            );

        piercing = stats.piercing;

        if (stats.lifespan > 0) Destroy(gameObject, stats.lifespan);

        if (hasAutoAim) AcquireAutoAimFacing();
    }

    public virtual void AcquireAutoAimFacing()
    {
        float aimAngle;

        EnemyStats[] targets = FindObjectsOfType<EnemyStats>();

        if( targets.Length > 0)
        {
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 difference = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        }
    }
}
