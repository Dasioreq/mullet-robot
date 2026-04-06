using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RobotAi : EnemyAI
{
    RobotAnims anims;
    [SerializeField] float attackCooldown;
    [SerializeField] float reloadCooldown;
    [SerializeField] int magSize;

    [SerializeField] ProjectylesAmmo bullet;
    [SerializeField] Transform aimingBone;
    [SerializeField] Vector3 bulletOffset;

    [SerializeField] float maxAngle;
    [SerializeField] float spreadAngle;
    [SerializeField] uint projectiles = 1;

    float cooldown = 0;
    int mag;

    protected override void Start()
    {
        base.Start();
        if(enemyActions is RobotAnims)
        {
            anims = (RobotAnims)enemyActions;
        }

        mag = magSize;
    }

    void Update()
    {
        if(cooldown > 0)
            cooldown -= Time.deltaTime;
        else if(anims.Angle <= maxAngle) 
            Attack();
    }

    public override void Attack()
    {
        base.Attack();
        for(int i = 0; i < projectiles; i++)
        {
            var proj = Instantiate(bullet.gameObject, aimingBone.position + aimingBone.rotation * bulletOffset, Quaternion.LookRotation(Quaternion.AngleAxis(Random.Range(0f, 180f), aimingBone.transform.up) * Quaternion.AngleAxis(Random.Range(-spreadAngle, spreadAngle), aimingBone.transform.right) * aimingBone.transform.up));
            proj.GetComponent<ProjectylesAmmo>().damage = damage;
        }
        mag--;
        if(mag == 0)
        {
            cooldown = reloadCooldown;
            mag = magSize;
        }
        else
            cooldown = attackCooldown;
    }
}
