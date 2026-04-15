using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

/// @brief Adds movement to basic Robots with turning animations
public class RobotAnims : EnemyActions
{
    [Serializable]
    class WeightedConstraint
    {
        public UnityEngine.Object constraintObject;
        [HideInInspector] public IRigConstraint Constraint => constraintObject as IRigConstraint;
        public AnimationCurve rigWeightOverTurn = AnimationCurve.Constant(0f, 1f, 1f);

        public void Evaluate(float t)
        {
            if(Constraint != null)
                Constraint.weight = rigWeightOverTurn.Evaluate(t);
        }
    }

    public Transform target;
    [SerializeField] Animator customAnimator;
    [SerializeField] public float range;
    [SerializeField] float marginAngle;
    [SerializeField] float maxOvershoot;
    [SerializeField] public Target aimingTarget;
    [SerializeField] LayerMask LOSLayers;
    [SerializeField] float turnTime = .75f;
    [SerializeField] WeightedConstraint[] weightedConstraints;
    bool checking = false;
    bool targeted = true;

    [SerializeField] Rig rig;

    float angleFromPlayer = 180;
    public float Angle {get {return angleFromPlayer;}}

    protected override void Start()
    {
        base.Start();
        if(customAnimator)
            anim = customAnimator;
        if(!target)
            target = GameObject.FindWithTag("Player").transform;
        if(!rig)
            rig = GetComponentInChildren<Rig>();
        GetComponent<RigBuilder>().Build();
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
                StartCoroutine(TryTurning(turnTime));

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

    /// @brief Handles turning towards the player
    /// 
    /// If the angle between itself and the target exceeds some given margin, it plays the animation for turning 90 degrees.
    public IEnumerator TryTurning(float checkDelay)
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
            StartCoroutine(CorrectRotation(angle - 90, turnTime));
        }
        else if(angle <= -marginAngle)
        {
            anim.SetTrigger("TrTurnLeft90");
            StartCoroutine(CorrectRotation(angle + 90, turnTime));
        }
        checking = false;
    }

    /// @brief Corrects the rotation
    /// 
    /// Tries to make the enemy point closer towards the target, by over/undershooting the rotation with a given max overshoot angle.
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

            foreach(var c in weightedConstraints)
            {
                c.Evaluate(t);
            }
            
            transform.Rotate(Vector3.up, (overshoot / time) * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        foreach(var c in weightedConstraints)
        {
            c.Constraint.weight = 1;
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