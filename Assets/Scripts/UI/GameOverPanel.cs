using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIView
{
    [Header("UI elements ")]
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _exitButton;

    private UIEventType _userEventType = UIEventType.Undefined;
    
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
        
        _userEventType = UIEventType.GameOverPlayAgainClick;
    }

    private void ExitClickHandler()
    {
        Hide();
        
        _userEventType = UIEventType.GameOverExitClick;
    }

    protected override void HandleHideCompleted()
    {
        base.HandleHideCompleted();
        
        InvokeOnUserEvent(_userEventType, null);

        _userEventType = UIEventType.Undefined;
    }
}
