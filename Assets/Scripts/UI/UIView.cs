using UI.Tween;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(UITweener))]
    public abstract class UIView : MonoBehaviour
    {
        protected UITweener _tweener;
    
        protected virtual void Awake()
        {
            _tweener = GetComponent<UITweener>();
        
            SetActive(false);
        }

        public abstract void Show();
    
        public virtual void Hide()
        {
            _tweener.Hide(HandleHideCompleted);
        }
    
        protected void SetActive(bool isActive) => gameObject.SetActive(isActive);

        protected virtual void HandleHideCompleted()
        {
            SetActive(false);
        }
    }
}