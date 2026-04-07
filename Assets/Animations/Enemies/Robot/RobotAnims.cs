using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class RobotAnims : EnemyActions
{
    public Transform target;
    [SerializeField] public float range;
    [SerializeField] float marginAngle;
    [SerializeField] float maxOvershoot;
    [SerializeField] public Target aimingTarget;
    [SerializeField] LayerMask LOSLayers;
    bool checking = false;
    bool targeted = true;

    Rig rig;

    float angleFromPlayer = 180;
    public float Angle {get {return angleFromPlayer;}}

    protected override void Start()
    {
        base.Start();
        if(!target)
            target = GameObject.FindWithTag("Player").transform;
        rig = GetComponentInChildren<Rig>();
    }

    void Update()
    {
        RaycastHit hit = new RaycastHit();
        if((transform.position - target.position).sqrMagnitude < range * range && !Physics.Linecast(transform.position + Vector3.up * 1.75f, target.position, out hit, LOSLayers) && !Physics.Linecast(target.position, transform.position + Vector3.up * 1.75f, out hit, LOSLayers))
        {
            if(!targeted)
            {
                StartCoroutine(LerpIKWeight(1f, .5f));
                targeted = true;
            }

            if(!checking)
                StartCoroutine(TryTurning(.75f));

            angleFromPlayer = aimingTarget.GetAngleFromTarget();
        }
        else
        {
            if(targeted)
            {
                StartCoroutine(LerpIKWeight(0f, .5f));
                targeted = false;
            }

            angleFromPlayer = 180;
        }
    }

    IEnumerator TryTurning(float checkDelay)
    {
        checking = true;
        float elapsed = 0;
        while(elapsed < checkDelay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        var dirToTarget = target.position - transform.position;

        var angle = Vector3.SignedAngle(transform.rotation * transform.forward, dirToTarget, Vector3.up);

        if(angle >= marginAngle)
        {
            anim.SetTrigger("TrTurnRight90");
            StartCoroutine(CorrectRotation(angle - 90, .75f));
        }
        else if(angle <= -marginAngle)
        {
            anim.SetTrigger("TrTurnLeft90");
            StartCoroutine(CorrectRotation(angle + 90, .75f));
        }
        checking = false;
    }

    IEnumerator CorrectRotation(float deltaAngle, float time)
    {
        float overshoot;
        
        if(deltaAngle <= 0)
            overshoot = Math.Max(-maxOvershoot, deltaAngle);
        else
            overshoot = Math.Min(maxOvershoot, deltaAngle);

        float elapsed = 0;
        while(elapsed < time)
        {
            var t = elapsed / time;
            
            transform.Rotate(Vector3.up, (overshoot / time) * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public override IEnumerator Attack()
    {
        anim.SetTrigger("TrFire");
        StartCoroutine(Flash(.1f));
        yield return new WaitForSeconds(attackSfxDelay);
        source.PlayOneShot(attackSound);
        yield break;
    }

    IEnumerator LerpIKWeight(float newWeight, float time)
    {
        float oldWeight = rig.weight;
        float elapsed = 0f;
        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            rig.weight = Mathf.Lerp(oldWeight, newWeight, elapsed / time);
            yield return null;
        }
        rig.weight = newWeight;
        yield break;
    }
}