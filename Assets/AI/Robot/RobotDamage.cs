using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RobotDamage : EnemyDamage
{
    [SerializeField] GameObject[] explosionParticles;
    [SerializeField] AudioClip deadSound;
    protected AudioSource source;
    [SerializeField] int maxGibs;
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    override protected void Start()
    {
        base.Start();
        source = GetComponent<AudioSource>();
    }

    override public void Destroy()
    {
        if(destroyed)
            return;
        base.Destroy();
        StartCoroutine(Explode(transform.position + Vector3.up));
        Destroy(GetComponent<EnemyAI>());
    }

    public IEnumerator Explode(Vector3 position)
    {
        // foreach (var obj in explosionParticles)
        // {
        //     foreach(var p in explosionParticles)
        //     {
        //         foreach(ParticleSystem particle in p.GetComponentsInChildren<ParticleSystem>())
        //         {
        //             Instantiate(particle, position, Quaternion.LookRotation(transform.up));
        //         }
        //     }
        // }

        SpawnGibs();

        foreach(Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
        foreach(Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        yield return new WaitForSeconds(0.05f);
        source.PlayOneShot(deadSound, 6);
        Destroy(gameObject, 2f);
        yield break;
    }

    void SpawnGibs()
    {
        var potentialGibs = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < maxGibs; i++)
        {
            GameObject theChosenOne = potentialGibs[Random.Range(0, potentialGibs.Length)].gameObject;
            GameObject gib = Instantiate(theChosenOne, theChosenOne.transform.position, theChosenOne.transform.rotation);
            gib.transform.localScale = transform.lossyScale;
            var collider = gib.AddComponent<MeshCollider>();
            collider.sharedMesh = gib.GetComponent<MeshFilter>().mesh;
            collider.convex = true;
            var rb = collider.AddComponent<Rigidbody>();
            rb.linearVelocity = Random.onUnitSphere * Random.Range(minForce, maxForce);
            // rb.isKinematic = true;
            // rb.useGravity = false;
            rb.angularVelocity = Random.insideUnitSphere;
            rb.linearDamping = 0;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            // var tc = gib.AddComponent<TerminalCancer>();
            // tc = new TerminalCancer(10, true);
            gib.transform.SetParent(null);
            gib.SetActive(true);
        }
    }
}
