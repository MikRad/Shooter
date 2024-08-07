﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class Pool<T> where T : MonoBehaviour
    {
        public bool IsAutoExpand { get; private set; }
        private T Prefab { get; }
        private Transform Container { get; }

        private List<T> _pool;

        public Pool(T prefab, int poolSize, bool isAutoExpand = true)
        {
            Prefab = prefab;
            Container = null;
            IsAutoExpand = isAutoExpand;

            InitPool(poolSize);
        }

        public Pool(T prefab, int poolSize, Transform container, bool isAutoExpand = true)
        {
            Prefab = prefab;
            Container = container;
            IsAutoExpand = isAutoExpand;
        
            InitPool(poolSize);
        }

        public bool HasFreeElement(out T element)
        {
            foreach (T elem in _pool)
            {
                if (!elem.gameObject.activeInHierarchy)
                {
                    elem.gameObject.SetActive(true);
                    element = elem;
                    return true;
                }
            }

            element = null;
            return false;
        }

        public T GetFreeElement()
        {
            if (HasFreeElement(out T element))
                return element;

            if (IsAutoExpand)
                return CreateElement(true);

            throw new Exception($"There is no free elements in pool of type {typeof(T)}");
        }
    
        private void InitPool(int poolSize)
        {
            int poolCapacity = (IsAutoExpand) ? poolSize * 2 : poolSize;
            
            _pool = new List<T>(poolCapacity);

            for (int i = 0; i < poolSize; i++)
                CreateElement();
        }

        private T CreateElement(bool isActive = false)
        {
            T element = Object.Instantiate(Prefab, Container);
            element.gameObject.SetActive(isActive);
            _pool.Add(element);
        
            return element;
        }
    }
}
