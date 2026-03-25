using UnityEngine;

public class ProjectylesAmmo : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed ;
    public float gravityMultiplier ;
    public float lifetime ;
    public float damage ;

    private float timeAlive = 0f;

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
