using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFactoryConfig", menuName = "ScriptableObjects/PlayerFactoryConfig", order = 1)]
public class PlayerFactoryConfig : ScriptableObject
{
    public Player playerPrefab;
}