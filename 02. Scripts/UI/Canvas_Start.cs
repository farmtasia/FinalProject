public class Canvas_Start : SceneUI
{
    public void OnClickSettingBtn()
    {
        UIManager.Instance.ShowUI<SettingsPopup>(UIs.Popup);
    }
}
