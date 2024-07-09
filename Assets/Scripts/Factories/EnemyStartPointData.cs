using UnityEngine;

public class EnemyStartPointData : MonoBehaviour
{
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemyPatrolPoint[] _patrolPoints;
    [SerializeField] private bool _isPatrol = true;

    public EnemyPatrolPoint[] PatrolPoints => _patrolPoints;
    public EnemyType EnemyType => _enemyType;
    public bool IsPatrol => _isPatrol;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < _patrolPoints.Length; i++)
        {
            Gizmos.DrawLine(_patrolPoints[i].transform.position,
                (i + 1) < _patrolPoints.Length
                    ? _patrolPoints[i + 1].transform.position
                    : _patrolPoints[0].transform.position);
        }
    }
}
