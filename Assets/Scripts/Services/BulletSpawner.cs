using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private Transform _bulletsContainer;
    [SerializeField] private Bullet[] _bulletPrefabs;
    [SerializeField] private int _bulletPoolSize = 20;

    private readonly Dictionary<BulletType, Pool<Bullet>> _bulletPoolsMap =
        new Dictionary<BulletType, Pool<Bullet>>();

    private void Awake()
    {
        FillBulletPoolsMap();
    }

    public void SpawnBullet(BulletType bulletType, Vector3 position, Quaternion rotation)
    {
        if (_bulletPoolsMap.TryGetValue(bulletType, out Pool<Bullet> bulletsPool))
        {
            bulletsPool.GetFreeElement().Init(position, rotation);
        }
        else
        {
            Debug.Log($"There is no pool for bullet of type {bulletType} !");
        }
    }
    
    private void FillBulletPoolsMap()
    {
        foreach (Bullet bullet in _bulletPrefabs)
        {
            _bulletPoolsMap.TryAdd(bullet.Type, new Pool<Bullet>(bullet, _bulletPoolSize, _bulletsContainer));
        }
    }
}