using UnityEngine;

[RequireComponent(typeof(EnemyActions))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] float damage;
    EnemyActions enemyActions;

    protected virtual void Start()
    {
        enemyActions = GetComponent<EnemyActions>();
    }

    public virtual void Attack() {StartCoroutine(enemyActions.Attack());}
    public virtual void Move() {StartCoroutine(enemyActions.Move());}
}
