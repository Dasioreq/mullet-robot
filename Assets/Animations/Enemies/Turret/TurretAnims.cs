using System.Collections;
using UnityEngine;

public class TurretAnims : EnemyActions
{
    public override IEnumerator Attack()
    {
        anim.SetTrigger("TrAttack");
        StartCoroutine(Flash(.1f));
        if(attackSound)
        {
            yield return new WaitForSeconds(attackSfxDelay);
            source.PlayOneShot(attackSound);
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
