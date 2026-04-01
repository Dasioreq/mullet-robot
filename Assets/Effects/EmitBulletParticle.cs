using UnityEngine;

public class EmitBulletParticle : MonoBehaviour
{
    ParticleSystem bulletParticle;
    Camera viewmodelCamera;

    void Start()
    {
        bulletParticle = GetComponent<ParticleSystem>();
        viewmodelCamera = GameObject.FindWithTag("ViewmodelCamera").GetComponent<Camera>();
    }

    public void Emit(Vector3 target)
    {
        var emitParams = new ParticleSystem.EmitParams();

        emitParams.velocity = (target - transform.position).normalized * 200;
        emitParams.applyShapeToPosition = false;
        emitParams.position = Camera.main.ViewportToWorldPoint(viewmodelCamera.WorldToViewportPoint(transform.position));

        bulletParticle.Emit(emitParams, 1);
    }

    public void EmitDirection(Vector3 direction)
    {
        var emitParams = new ParticleSystem.EmitParams();

        emitParams.velocity = direction * 200;
        emitParams.applyShapeToPosition = false;
        emitParams.position = Camera.main.ViewportToWorldPoint(viewmodelCamera.WorldToViewportPoint(transform.position));

        bulletParticle.Emit(emitParams, 1);
    }
}
