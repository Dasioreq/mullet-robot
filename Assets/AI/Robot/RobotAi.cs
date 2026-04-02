using Unity.VisualScripting;
using UnityEngine;

public class RobotAi : EnemyAI
{
    RobotAnims anims;
    [SerializeField] float attackCooldown;
    [SerializeField] float reloadCooldown;
    [SerializeField] int magSize;

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
        Debug.Log(anims.Angle);
        if(cooldown > 0)
            cooldown -= Time.deltaTime;
        else
        {
            if(anims.Angle <= 30)
            {
                StartCoroutine(anims.Attack());
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
    }
}
