using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelLoadProgressPanel : UIView
    {
        [SerializeField] private Image _loadProgress;
    
        public override void Show()
        {
            _loadProgress.fillAmount = 0f;
        
            SetActive(true);

            _tweener.Show();
        }

        public void SetProgressValue(float progressValue)
        {
            _loadProgress.fillAmount = progressValue;
        }
    }
}
