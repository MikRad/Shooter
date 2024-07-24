using DG.Tweening;
using UnityEngine;

namespace PickupItems
{
    public class PickupItemAnimator : MonoBehaviour
    {
        [Header("Rotation Settings")] 
        [SerializeField] private bool _enableRotation = true;
        [SerializeField] private float _rotationDuration = 2f;
        [SerializeField] private Vector3 _rotationAxis = new Vector3(0f, 0f, 1f);

        [Header("Pulsation Settings")] 
        [SerializeField] private bool _enablePulsation = true;
        [SerializeField] private float _pulsationDuration = 0.5f;
        [SerializeField] private Vector3 _pulsationScale = new Vector3(1.2f, 1.2f, 1f);

        private Tween _rotationTween;
        private Tween _puslationTween;

        private void OnEnable()
        {
            ResumeTweens();
        }

        private void OnDisable()
        {
            PauseTweens();
        }
    
        private void Start()
        {
            if (_enableRotation)
            {
                _rotationTween = transform.DORotate(_rotationAxis * 360f, _rotationDuration, RotateMode.FastBeyond360)
                    .SetLoops(-1, LoopType.Restart)
                    .SetEase(Ease.Linear);
            }

            if (_enablePulsation)
            {
                _puslationTween = transform.DOScale(_pulsationScale, _pulsationDuration)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            }
        }

        private void OnDestroy()
        {
            _rotationTween?.Kill();
            _puslationTween?.Kill();
        }

        private void ResumeTweens()
        {
            _rotationTween?.Play();
            _puslationTween?.Play();
        }
    
        private void PauseTweens()
        {
            _rotationTween?.Pause();
            _puslationTween?.Pause();
        }
    }
}