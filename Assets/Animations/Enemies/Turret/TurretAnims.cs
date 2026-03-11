using System.Collections;
using UnityEngine;

public class TurretAnims : EnemyActions
{
    [SerializeField] AudioClip altAttackSound;
    public override IEnumerator Attack()
    {
        anim.SetTrigger("TrAttack");
        StartCoroutine(Flash(.1f));
        if(attackSound)
        {
            yield return new WaitForSeconds(attackSfxDelay);
            int chance = Random.Range(0, 2);
            if (chance < 1)
            {
                source.PlayOneShot(attackSound,4);
            }
            else
            {
                source.PlayOneShot(altAttackSound,4);
            }
        }
        yield break;
    }

    public override IEnumerator Move()
    {
        if(moveSound)
        {
            yield return new WaitForSeconds(moveSfxDelay);
            source.PlayOneShot(moveSound);
        }
        yield break;
    }
}
