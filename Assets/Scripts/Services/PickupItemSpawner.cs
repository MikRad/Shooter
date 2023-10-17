using System.Collections.Generic;
using UnityEngine;

public class PickupItemSpawner : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private Transform _itemsContainer;
    [SerializeField] private BasePickupItem[] _itemPrefabs;
    [SerializeField] private int _itemPoolSize = 10;

    private readonly Dictionary<PickupItemType, Pool<BasePickupItem>> _itemPoolsMap = new Dictionary<PickupItemType, Pool<BasePickupItem>>();

    private void Awake()
    {
        FillItemPoolsMap();
    }

    public void SpawnItem(PickupItemType pickupItemType, Vector3 position)
    {
        if (_itemPoolsMap.TryGetValue(pickupItemType, out Pool<BasePickupItem> itemPool))
        {
            itemPool.GetFreeElement().Init(position, Quaternion.identity);
        }
        else
        {
            Debug.Log($"There is no pool for item of type {pickupItemType} !");
        }
    }
    
    private void FillItemPoolsMap()
    {
        foreach (BasePickupItem item in _itemPrefabs)
        {
            if (!_itemPoolsMap.ContainsKey(item.Type))
            {
                _itemPoolsMap.Add(item.Type, new Pool<BasePickupItem>(item, _itemPoolSize, _itemsContainer));
            }
        }
    }
}
