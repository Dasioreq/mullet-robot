using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class RobotAnims : MonoBehaviour
{
    public Transform target;
    Animator anim;
    [SerializeField] float marginAngle;
    [SerializeField] float maxOvershoot;
    bool checking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(!checking)
            StartCoroutine(TryTurning(.75f));
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
}