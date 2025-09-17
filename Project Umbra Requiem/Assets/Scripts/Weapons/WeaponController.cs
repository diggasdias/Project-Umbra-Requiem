using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Stats")]

    public GameObject prefab;
    public float damage;
    public float speed;
    public float cooldownDuration;
    float currentCooldown;
    public int pierce;

    protected virtual void Start()
    {
            currentCooldown = cooldownDuration;
    }

    protected virtual private void Update()
    {
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        currentCooldown = cooldownDuration;
    }
}
