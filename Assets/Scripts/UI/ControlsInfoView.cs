public class ControlsInfoView : UIView
{
    public override void Show()
    {
        SetActive(true);

        _tweener.Show(OnShowCompleted);
    }

    private void OnShowCompleted()
    {
        SetActive(false);
    }
}