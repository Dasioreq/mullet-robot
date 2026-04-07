using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RobotDamage : EnemyDamage
{
    [SerializeField] GameObject[] explosionParticles;
    [SerializeField] AudioClip deadSound;
    protected AudioSource source;

    [Header("Giblet settings")]
    [SerializeField] int maxGibs;
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    [SerializeField] int gibLayer;
    [SerializeField] ParticleSystem[] gibParticles;

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
        foreach (var obj in explosionParticles)
        {
            foreach(var p in explosionParticles)
            {
                foreach(ParticleSystem particle in p.GetComponentsInChildren<ParticleSystem>())
                {
                    Instantiate(particle, position, Quaternion.LookRotation(transform.up));
                }
            }
        }

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
        var potentialGibs = GetComponentsInChildren<MeshRenderer>().Where(r =>
            r.bounds.size.sqrMagnitude > .2f).ToList();
        for(int i = 0; i < maxGibs; i++)
        {
            if(potentialGibs.Count <= 0)
                return;
            int index = Random.Range(0, potentialGibs.Count - 1);
            GameObject theChosenOne = potentialGibs[index].gameObject; potentialGibs.RemoveAt(index);
            GameObject gib = Instantiate(theChosenOne, theChosenOne.transform.position, theChosenOne.transform.rotation);
            gib.transform.localScale = transform.lossyScale;
            var collider = gib.AddComponent<MeshCollider>();
            collider.sharedMesh = gib.GetComponent<MeshFilter>().mesh;
            collider.convex = true;
            var rb = collider.AddComponent<Rigidbody>();
            rb.linearVelocity = Random.onUnitSphere * Random.Range(minForce, maxForce);
            collider.excludeLayers |= 1 << gibLayer;
            gib.layer = gibLayer;
            rb.angularVelocity = Random.insideUnitSphere;
            rb.linearDamping = 0;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rb.GetComponent<Renderer>().material.SetFloat("_Cull", 0);
            var tc = gib.AddComponent<TerminalCancer>().Init(Random.Range(5f, 10f));
            var p = gibParticles[Random.Range(0, gibParticles.Length)];
            GameObject particleEmitter = p? Instantiate(p.gameObject, gib.transform) : null;
            gib.transform.SetParent(null);
            gib.SetActive(true);
        }
    }
}
