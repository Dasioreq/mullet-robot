using UnityEngine;

public class TerminalCancer : MonoBehaviour
{
    public bool destroy = false;
    public float timeTilDeath;
    float deathTimer;

    public TerminalCancer(float time, bool destroy)
    {
        timeTilDeath = time;
        this.destroy = destroy;
        deathTimer = time;
    }

    public TerminalCancer Init(float time, bool destroy)
    {
        timeTilDeath = time;
        this.destroy = destroy;
        deathTimer = time;
        return this;
    }

    void Start()
    {
        deathTimer = timeTilDeath;
    }

    void Update()
    {
        if(deathTimer > 0)
            deathTimer -= Time.deltaTime;
        else
        {
            if(destroy)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}
