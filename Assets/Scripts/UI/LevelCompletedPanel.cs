using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedPanel : UIViewInteractable
{
    [Header("UI elements ")]
    [SerializeField] private Button _continueButton;

    protected override void AddElementsListeners()
    {
        _continueButton.onClick.AddListener(ContinueClickHandler);
    }

    protected override void RemoveElementsListeners()
    {
        _continueButton.onClick.RemoveListener(ContinueClickHandler);
    }

    protected override void SetEnableElements(bool isEnabled)
    {
        _continueButton.enabled = isEnabled;
    }
    
    private void ContinueClickHandler()
    {
        Hide();
    }

    protected override void HandleHideCompleted()
    {
        base.HandleHideCompleted();

        EventBus.Get.RaiseEvent(this, new LevelCompletedPanelClosedEvent(UserAction.Okay));
    }
    
    public enum UserAction
    {
        Okay
    }
}