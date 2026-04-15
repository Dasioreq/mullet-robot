using UnityEngine;

/// @class EmitBulletParticle 
/// @brief Script for ~~emmiting~~ emitting bullet particles when a gun is fired
public class EmitBulletParticle : MonoBehaviour
{
    ParticleSystem bulletParticle;
    Camera viewmodelCamera;

    void Start()
    {
        bulletParticle = GetComponent<ParticleSystem>();
        viewmodelCamera = GameObject.FindWithTag("ViewmodelCamera").GetComponent<Camera>();
    }

    /// @brief Instantiates the bullet particle at a pre-determined position, accounting for the difference in the viewmodel and player cameras' FOV-s. The particle will point to a given destination.
    public void Emit(Vector3 target)
    {
        var emitParams = new ParticleSystem.EmitParams();

        emitParams.velocity = (target - transform.position).normalized * 200;
        emitParams.applyShapeToPosition = false;
        emitParams.position = Camera.main.ViewportToWorldPoint(viewmodelCamera.WorldToViewportPoint(transform.position));

        bulletParticle.Emit(emitParams, 1);
    }

    /// @brief Instantiates the bullet particle at a pre-determined position, accounting for the difference in the viewmodel and player cameras' FOV-s. The particle will point in a given direction.
    public void EmitDirection(Vector3 direction)
    {
        var emitParams = new ParticleSystem.EmitParams();

        emitParams.velocity = direction * 200;
        emitParams.applyShapeToPosition = false;
        emitParams.position = Camera.main.ViewportToWorldPoint(viewmodelCamera.WorldToViewportPoint(transform.position));

        bulletParticle.Emit(emitParams, 1);
    }
}
