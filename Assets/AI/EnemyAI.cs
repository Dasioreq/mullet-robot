using UnityEngine;

/// @class EnemyAI
/// @brief Base class for enemy AI systems
[RequireComponent(typeof(EnemyActions))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float damage;
    protected EnemyActions enemyActions;

    protected virtual void Start()
    {
        enemyActions = GetComponent<EnemyActions>();
    }

    /// @brief Defines the enemy's attack
    public virtual void Attack() {StartCoroutine(enemyActions.Attack());}
    /// @brief Defines the enemy's movement
    public virtual void Move() {StartCoroutine(enemyActions.Move());}
}
