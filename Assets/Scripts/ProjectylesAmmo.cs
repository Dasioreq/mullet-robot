using UnityEngine;

/// @class ProjectylesAmmo
/// @brief Base class for projectiles afected by gravity and other external forces
public class ProjectylesAmmo : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed ;
    public float gravityMultiplier ;
    public float lifetime ;
    public float damage ;

    public LayerMask collisionLayers;

    protected Vector3 previousFramePosition;

    protected Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Destroy(gameObject, lifetime);
        previousFramePosition = transform.position;
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    void LateUpdate()
    {
        CheckForCollision();
        previousFramePosition = transform.position;
    }

    /// @brief Performs a Linecast collision check
    public void CheckForCollision()
    {
        RaycastHit hit;
        if(Physics.Linecast(previousFramePosition, transform.position, out hit, collisionLayers) || Physics.Linecast(transform.position, previousFramePosition, out hit, collisionLayers))
        {
            OnHit(hit);
            Destroy(gameObject);
        }
    }

    /// @brief Called when a collision occurs
    protected virtual void OnHit(RaycastHit hit)
    {
        IHittable hittable;
        if((hittable = hit.collider.gameObject.GetComponentInParent<IHittable>()) != null)
        {
            hittable.OnHit(hit, damage);
        }
    }
}
