using System;
using UnityEngine;

public class PlayerExpLevel : MonoBehaviour
{
    private PlayerData playerData => DataManager.Instance.curData.playerData;
    public event Action<int> OnLevelChanged;
    public float expToNextLevel = 100; // 레벨업에 필요한 기본 경험치
    public SEType levelUpSoundEffect = SEType.LEVELUP;
    public GameObject levelUpEffectPrefab;

    public void GetExp(float amount) // 경험치 획득 메서드
    {
        playerData.currentExp += amount;
        Debug.Log($"획득경험치 : {amount}  현재경험치: {playerData.currentExp}");

        if (playerData.currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp() // 레벨업 메서드
    {
        playerData.currentExp -= expToNextLevel;
        playerData.level++;
        expToNextLevel = Mathf.FloorToInt(expToNextLevel * 1.5f); // 레벨업에 필요한 경험치 증가. 임시로 1.5로 설정.
        SoundManager.Instance.PlayEffect(levelUpSoundEffect);
        OnLevelChanged?.Invoke(playerData.level);
        Debug.Log($"레벨업!! 현재 레벨: {playerData.level}");

        // 레벨업 표시
        ShowLevelUpEffect();
    }

    private void ShowLevelUpEffect()
    {
        GameObject levelUpImageObject = Instantiate(levelUpEffectPrefab, GameManager.Instance.Player.levelUpEffectPosition);
        levelUpImageObject.SetActive(true);
    }
}
