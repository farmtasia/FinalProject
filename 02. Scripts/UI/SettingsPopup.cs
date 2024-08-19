using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPopup : PopupUI
{
    public Slider BGMSlider;
    public Slider SESlider;
    public Button SettingCloseBtn;
    public Image savePanel;
    public Image saveEndPanel;

    private void Start()
    {
        BGMSlider.onValueChanged.AddListener((value) => { SoundManager.Instance.SetVolume(Sound.BGM, value); });
        SESlider.onValueChanged.AddListener((value) => { SoundManager.Instance.SetVolume(Sound.EFFECT, value); });
        
        SettingCloseBtn.onClick.AddListener(Canvas_Main.OnCloseBtn);
        SettingCloseBtn.onClick.AddListener(ClosePanel);

        BGMSlider.value = SoundManager.Instance.GetVolume(Sound.BGM);
        SESlider.value = SoundManager.Instance.GetVolume(Sound.EFFECT);

        //gameObject.SetActive(false);
    }

    public void ShowSavePanel()
    {
        if (SceneManager.GetActiveScene().name != SceneName.StartScene.ToString())
        {
            savePanel.gameObject.SetActive(true);
            StartCoroutine(ShowPanelTime(savePanel.gameObject, 1f));
        }
    }

    public void ShowSaveEndPanel()
    {
        if (SceneManager.GetActiveScene().name != SceneName.StartScene.ToString())
        {
            saveEndPanel.gameObject.SetActive(true);
            SettingCloseBtn.onClick.RemoveAllListeners();
            StartCoroutine(ShowPanelTime(saveEndPanel.gameObject, 1.3f));
        }
    }

    private IEnumerator ShowPanelTime(GameObject panel, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        panel.SetActive(false);

        if (panel.name == saveEndPanel.name)
            GameManager.Instance.SaveDataAndExit();
    }

    private void ClosePanel()
    {
        gameObject.SetActive(false);

        if (savePanel.gameObject.activeSelf)
            savePanel.gameObject.SetActive(false);
    }
}
