using DI.Services;
using Factories.Config;
using Units.Player;
using UnityEngine;

namespace Factories
{
    public class PlayerFactory
    {
        private const string ConfigPath = "Configs/PlayerFactoryConfig";
        private readonly DIContainer _diContainer;
        private readonly PlayerFactoryConfig _config;
    
        public PlayerFactory(DIContainer diContainer)
        {
            _diContainer = diContainer;

            IResourcesDataProvider dataProvider = _diContainer.Resolve<IResourcesDataProvider>();
            _config = dataProvider.LoadResource<PlayerFactoryConfig>(ConfigPath);
        }

        public Player CreatePlayer(Vector3 position)
        {
            Player player = Object.Instantiate(_config.playerPrefab, position, Quaternion.identity);
            player.Init(_diContainer);

            return player;
        }
    }
}
