using UnityEngine;
using DG.Tweening;

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

    private void Start()
    {
        if (_enableRotation)
        {
            transform.DORotate(_rotationAxis * 360f, _rotationDuration, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }

        if (_enablePulsation)
        {
            transform.DOScale(_pulsationScale, _pulsationDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
}