using UnityEngine;

public class TerminalCancer : MonoBehaviour
{
    public bool destroy = false;
    public float timeTilDeath;
    float deathTimer;

    void Start()
    {
        deathTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        deathTimer += Time.deltaTime;
        if(deathTimer >= timeTilDeath)
            if(destroy)
                Destroy(gameObject);
            else
            {
                deathTimer = 0.0f;
                gameObject.SetActive(false);
            }
    }
}
