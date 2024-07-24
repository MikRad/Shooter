using System;
using System.Collections;
using UnityEngine;

namespace Vfx
{
    public class VfxLifeTimeChecker : MonoBehaviour
    {
        [SerializeField] private float _lifeTime = 1.5f;

        private Coroutine _lifeTimeUpdater;

        public event Action OnLifeTimeExpired;
    
        public void Init()
        {
            _lifeTimeUpdater = StartCoroutine(UpdateLifeTime());
        }
    
        private IEnumerator UpdateLifeTime()
        {
            yield return new WaitForSeconds(_lifeTime);

            StopCoroutine(_lifeTimeUpdater);
        
            OnLifeTimeExpired?.Invoke();
        }
    }
}
