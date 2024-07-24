using UnityEngine;

namespace Services
{
    public class Boot : MonoBehaviour
    {
        [SerializeField] private Game _gamePrefab;

        private void Start()
        {
            Game game = Instantiate(_gamePrefab);
            DontDestroyOnLoad(game);
        
            game.Init();
        }
    }
}
