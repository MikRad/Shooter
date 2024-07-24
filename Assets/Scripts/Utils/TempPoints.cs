using UnityEngine;

namespace Utils
{
    public class TempPoints : MonoBehaviour
    {
        private static bool _isInitialized;
        private static Transform _container;
    
        public static Transform Container
        {
            get
            {
                if (!_isInitialized)
                {
                    GameObject gObj = new GameObject($"--- {nameof(TempPoints)} ---");
                    gObj.AddComponent<TempPoints>();
                    _container = gObj.transform;
                    _isInitialized = true;
                }

                return _container;
            }
        }

        private void OnDestroy()
        {
            _isInitialized = false;
            _container = null;
        }
    }
}
