using UnityEngine;

public class gun : MonoBehaviour
{
    [SerializeField] private float gunDamage;
    [SerializeField] public Camera cam;
    [SerializeField] private float fireCooldown;

    private float cooldown = .0f;

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Vector3 origin = cam.transform.position;
                Vector3 direction = cam.transform.forward;

                if (Physics.Raycast(origin, direction, out hit))
                {
                    Debug.Log(hit.collider.gameObject.name + origin);
                }

                cooldown = fireCooldown;
            }
        }
    }
}
