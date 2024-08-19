using TMPro;
using UnityEngine;

public class InfoBox : MonoBehaviour
{
    // TODO: 캐릭터 이미지
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI farmNameText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI levelText;

    public void Init()
    {
        GameManager.Instance.Player.gold.OnGoldChanged -= UpdateGoldText;
        GameManager.Instance.Player.expLevel.OnLevelChanged -= UpdateLevelText;
        GameManager.Instance.Player.gold.OnGoldChanged += UpdateGoldText;
        GameManager.Instance.Player.expLevel.OnLevelChanged += UpdateLevelText;
        userNameText.text = DataManager.Instance.curData.nameData.userName;
        farmNameText.text = DataManager.Instance.curData.nameData.farmName + " 농장";
        UpdateGoldText(DataManager.Instance.curData.playerData.gold);
        UpdateLevelText(DataManager.Instance.curData.playerData.level);
    }

    private void UpdateGoldText(int gold)
    {
        goldText.text = gold.ToString() + " G";
    }

    private void UpdateLevelText(int level)
    {
        levelText.text = "Lv. " + level.ToString();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance.Player.gold != null || GameManager.Instance.Player.expLevel)
        {
            GameManager.Instance.Player.gold.OnGoldChanged -= UpdateGoldText;
            GameManager.Instance.Player.expLevel.OnLevelChanged -= UpdateLevelText;
        }
    }
}
