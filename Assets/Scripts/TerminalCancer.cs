using UnityEngine;

public class TerminalCancer : MonoBehaviour
{
    public float timeTilDeath;

    public TerminalCancer(float time)
    {
        timeTilDeath = time;
    }

    public TerminalCancer Init(float time)
    {
        timeTilDeath = time;
        return this;
    }

    void Start()
    {
        Destroy(gameObject, timeTilDeath);
    }
}
