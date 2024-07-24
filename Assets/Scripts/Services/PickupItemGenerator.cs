using System;
using PickupItems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services
{
    public class PickupItemGenerator : MonoBehaviour
    {
        [Header("Item generation")]
        [Range(0, 100)]
        [SerializeField] private int _itemGenerationProbability;
        [SerializeField] private PickupGenerationInfo[] _pickupGenerationInfos;
    
        private PickupItemSpawner _pickupItemSpawner;

        public void Init(PickupItemSpawner itemSpawner)
        {
            _pickupItemSpawner = itemSpawner;
        }
    
        public void TryGenerateItem(Vector3 position)
        {
            if ((_pickupGenerationInfos == null) || (_pickupGenerationInfos.Length == 0))
                return;

            int randProbability = Random.Range(1, 101);
            if(randProbability <= _itemGenerationProbability)
            {
                int totalWeight = 0;
                foreach (PickupGenerationInfo info in _pickupGenerationInfos)
                {
                    totalWeight += info.probabilityWeight;
                }

                int randWeight = Random.Range(0, totalWeight + 1);
                int cumulativeWeight = 0;
                for (int i = 0; i < _pickupGenerationInfos.Length; i++)
                {
                    cumulativeWeight += _pickupGenerationInfos[i].probabilityWeight;
                    if (cumulativeWeight >= randWeight)
                    {
                        _pickupItemSpawner.SpawnItem(_pickupGenerationInfos[i].itemType, position);
                        return;
                    }
                }
            }
        }
    
        [Serializable]
        private struct PickupGenerationInfo
        {
            public PickupItemType itemType;

            [Range(1, 10)]
            public int probabilityWeight;
        }
    
    }
}
