using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyStartPointData : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private EnemyPatrolPoint[] _patrolPoints;
    [SerializeField] private bool _isPatrol = true;
    [SerializeField] private EnemyIconInfo[] _enemyIconsInfos;

    public EnemyPatrolPoint[] PatrolPoints => _patrolPoints;
    public EnemyType EnemyType => _enemyType;
    public bool IsPatrol => _isPatrol;

    private void OnValidate()
    {
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (_renderer == null)
            return;
        
        for (int i = 0; i < _enemyIconsInfos.Length; i++)
        {
            if (_enemyIconsInfos[i].type == _enemyType)
            {
                _renderer.sprite = _enemyIconsInfos[i].icon;
                break;
            }
        }
    }
    
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
    
    [Serializable]
    private struct EnemyIconInfo
    {
        public EnemyType type;
        public Sprite icon;
    }
}

