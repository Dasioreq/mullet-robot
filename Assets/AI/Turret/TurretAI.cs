using UnityEngine;

[RequireComponent(typeof(TurretRotateTowardsPlayer))]
public class TurretAI : EnemyAI
{
    TurretRotateTowardsPlayer rotationScript;
    [SerializeField] float rps;
    float attackTime;
    [SerializeField] float attackAngle;
    float attackTimer = 0;
    [SerializeField] ProjectylesAmmo bullet;
    [SerializeField] Transform aimingBone;
    [SerializeField] Vector3 bulletOffset;
    [SerializeField] float spreadAngle;

    protected override void Start()
    {
        base.Start();
        rotationScript = GetComponent<TurretRotateTowardsPlayer>();
        attackTime = 1 / rps;
    }

    void Update()
    {
        Move();

        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }

        if(rotationScript.advanceTowardsPlayer() <= attackAngle && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackTime;
        }
    }

    public override void Move()
    {
        base.Move();
    }

    public override void Attack()
    {
        base.Attack();
        Instantiate(bullet.gameObject, aimingBone.position + aimingBone.rotation * bulletOffset, Quaternion.LookRotation(Quaternion.AngleAxis(Random.Range(0f, 180f), aimingBone.transform.up) * Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), aimingBone.transform.right) * aimingBone.transform.up));
    }
}
