public class Canvas_Guide : PopupUI
{
    public void OnClickGuideBtn()
    {
        UIManager.Instance.ShowUI<Canvas_Guide>(UIs.Popup);
    }
}
