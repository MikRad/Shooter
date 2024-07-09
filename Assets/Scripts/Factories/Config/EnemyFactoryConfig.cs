using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFactoryConfig", menuName = "ScriptableObjects/EnemyFactoryConfig", order = 1)]
public class EnemyFactoryConfig : ScriptableObject
{
    [SerializeField] private EnemyCreateInfo[] _createInfos;

    private readonly Dictionary<EnemyType, EnemyUnit> _enemyPrefabsMap = new Dictionary<EnemyType, EnemyUnit>();
    
    private void OnEnable()
    {
        FillCreateInfoMap();
    }

    public EnemyUnit GetPrefab(EnemyType enemyType)
    {
        return _enemyPrefabsMap.TryGetValue(enemyType, out EnemyUnit enemy) ? enemy : null;
    }
    
    private void FillCreateInfoMap()
    {
        _enemyPrefabsMap.Clear();

        if (_createInfos == null)
            return;

        foreach (EnemyCreateInfo info in _createInfos)
        {
            _enemyPrefabsMap.TryAdd(info._type, info._enemyPrefab);
        }
    }
}