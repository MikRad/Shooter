using Events.Services;
using Events.Services.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameCompletedPanel : UIViewInteractable
    {
        [Header("UI elements")]
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _exitButton;

        private UserAction _userAction;

        protected override void AddElementsListeners()
        {
            _playAgainButton.onClick.AddListener(PlayAgainClickHandler);
            _exitButton.onClick.AddListener(ExitClickHandler);
        }

        protected override void RemoveElementsListeners()
        {
            _playAgainButton.onClick.RemoveListener(PlayAgainClickHandler);
            _exitButton.onClick.RemoveListener(ExitClickHandler);
        }

        protected override void SetEnableElements(bool isEnabled)
        {
            _playAgainButton.enabled = isEnabled;
            _exitButton.enabled = isEnabled;
        }
    
        private void PlayAgainClickHandler()
        {
            Hide();

            _userAction = UserAction.PlayAgain;
        }

        private void ExitClickHandler()
        {
            Hide();
        
            _userAction = UserAction.Exit;
        }

        protected override void HandleHideCompleted()
        {
            base.HandleHideCompleted();

            EventBus.Get.RaiseEvent(this, new GameCompletedPanelClosedEvent(_userAction));
        
            _userAction = UserAction.Undefined;
        }
    
        public enum UserAction
        {
            Undefined,
            PlayAgain,
            Exit
        }
    }
}
