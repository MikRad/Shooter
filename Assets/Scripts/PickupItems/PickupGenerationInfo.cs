using System;
using UnityEngine;

[Serializable]
public struct PickupGenerationInfo
{
    public PickupItemType itemType;

    [Range(1, 10)]
    public int probabilityWeight;
}
