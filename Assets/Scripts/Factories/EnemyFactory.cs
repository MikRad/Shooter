using System.Collections.Generic;
using DI;
using DI.Services;
using Factories.Config;
using Factories.Utils;
using Units.Abstractions;
using Units.Enemy;
using UnityEngine;
using Random = System.Random;

namespace Factories
{
    public class EnemyFactory
    {
        private const string ConfigPath = "Configs/EnemyFactoryConfig";
        private readonly DIContainer _diContainer;
        private EnemyFactoryConfig _config;
        private readonly List<Vector3> _allPatrolPositions = new List<Vector3>();
    
        private const int MinPatrolPointsNumber = 2;
        private const int DefaultPatrolPointsNumber = 4;
    
        public EnemyFactory(DIContainer diContainer)
        {
            _diContainer = diContainer;
        
            LoadConfig();
        }

        public EnemyUnit CreateEnemy(EnemyStartPoint data)
        {
            EnemyUnit enemy = Object.Instantiate(_config.GetPrefab(data.EnemyType), data.transform.position, Quaternion.identity);
            enemy.Init(_diContainer);
        
            if (data.IsPatrol && _allPatrolPositions.Count >= MinPatrolPointsNumber)
            {
                InitEnemyPatrolPositions(enemy, data);
            }
        
            return enemy;
        }

        private void InitEnemyPatrolPositions(EnemyUnit enemy, EnemyStartPoint data)
        {
            List<Vector3> positions = new List<Vector3>();

            if (data.PatrolPoints.Length >= MinPatrolPointsNumber)
            {
                HashSet<EnemyPatrolPoint> uniquePoints = new HashSet<EnemyPatrolPoint>(data.PatrolPoints);
                foreach (EnemyPatrolPoint point in uniquePoints)
                {
                    positions.Add(point.transform.position);
                }
                enemy.SetPatrolPositions(positions);
                return;
            }

            if (_allPatrolPositions.Count <= DefaultPatrolPointsNumber)
            {
                enemy.SetPatrolPositions(_allPatrolPositions);
                return;
            }
        
            HashSet<Vector3> uniquePositions = new HashSet<Vector3>();
        
            Random random = new Random();
            while (uniquePositions.Count < DefaultPatrolPointsNumber)
            {
                int rndIdx = random.Next(0, _allPatrolPositions.Count);
                uniquePositions.Add(_allPatrolPositions[rndIdx]);
            }
        
            enemy.SetPatrolPositions(new List<Vector3>(uniquePositions));
        }

        public void InitAllPatrolPositions(EnemyPatrolPoint[] patrolPoints)
        {
            foreach (EnemyPatrolPoint patrolPoint in patrolPoints)
            {
                _allPatrolPositions.Add(patrolPoint.transform.position);    
            }
        }
    
        private void LoadConfig()
        {
            _config = Resources.Load<EnemyFactoryConfig>(ConfigPath);        
        }
    }
}