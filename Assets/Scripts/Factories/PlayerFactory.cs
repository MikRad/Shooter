﻿using UnityEngine;

public class PlayerFactory
{
    private const string ConfigPath = "Configs/PlayerFactoryConfig";
    private readonly DIContainer _diContainer;
    private PlayerFactoryConfig _config;
    
    public PlayerFactory(DIContainer diContainer)
    {
        _diContainer = diContainer;
        
        LoadConfig();
    }

    public Player CreatePlayer(Vector3 position)
    {
        Player player = GameObject.Instantiate(_config.playerPrefab, position, Quaternion.identity);
        player.Init(_diContainer);

        return player;
    }

    private void LoadConfig()
    {
        _config = Resources.Load<PlayerFactoryConfig>(ConfigPath);        
    }
}
