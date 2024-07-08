﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class VfxSpawner : MonoBehaviour
{
    [Header("Base settings")]
    [SerializeField] private Transform _vfxContainer;
    [SerializeField] private BaseVfx[] _vfxPrefabs;
    [SerializeField] private int _vfxPoolSize = 10;

    private readonly Dictionary<VfxType, Pool<BaseVfx>> _vfxPoolsMap = new Dictionary<VfxType, Pool<BaseVfx>>();
    
    private void Awake()
    {
        FillVfxPoolsMap();
        
        EventBus.Get.Subscribe<VfxNeededEvent>(HandleVfxNeeded);
    }

    private void OnDestroy()
    {
        EventBus.Get.Unsubscribe<VfxNeededEvent>(HandleVfxNeeded);
    }

    public void SpawnVfx(VfxType vfxType, Vector3 position, Quaternion rotation)
    {
        if (_vfxPoolsMap.TryGetValue(vfxType, out Pool<BaseVfx> vfxPool))
        {
            vfxPool.GetFreeElement().Init(position, rotation);
        }
        else
        {
            Debug.Log($"There is no pool for vfx of type {vfxType} !");
        }
    }
    
    private void HandleVfxNeeded(ref VfxNeededEvent ev)
    {
        SpawnVfx(ev.VfxType, ev.TargetTransform.position, ev.TargetTransform.rotation);
    }
    
    private void FillVfxPoolsMap()
    {
        foreach (BaseVfx vfx in _vfxPrefabs)
        {
            if (!_vfxPoolsMap.ContainsKey(vfx.Type))
                _vfxPoolsMap.Add(vfx.Type, new Pool<BaseVfx>(vfx, _vfxPoolSize, _vfxContainer));
        }
    }
}