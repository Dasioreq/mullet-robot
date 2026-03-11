using UnityEngine;

[RequireComponent(typeof(TurretRotateTowardsPlayer))]
public class TurretAI : EnemyAI
{
    TurretRotateTowardsPlayer rotationScript;
    [SerializeField] float rps;
    float attackTime;
    [SerializeField] float attackAngle;
    float attackTimer = 0;

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
    }
}
