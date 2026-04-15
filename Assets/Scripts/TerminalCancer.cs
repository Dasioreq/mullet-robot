using UnityEngine;

/// @class TerminalCancer
/// @brief Script that ~~kills~~ Destroys its own GameObject after a set period of time.
public class TerminalCancer : MonoBehaviour
{
    public float timeTilDeath;

    public TerminalCancer(float time)
    {
        timeTilDeath = time;
    }

    /// @brief a semi-constructor that makes adding thes script as a Component much easier
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
