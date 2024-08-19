using System;
using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    public int gold = 100; // 초기 골드
    public event Action<int> OnGoldChanged;

    public int currentGold => DataManager.Instance.curData.playerData.gold; // 현재 골드를 반환하는 프로퍼티

    public void AddGold(int amount) // 골드 추가 메서드
    {
        UpdateGold(amount);
        Debug.Log($"획득 골드 : {amount} 현재 골드 : {currentGold}");
    }

    public void SubtractGold(int amount) // 골드 감소 메서드
    {
        if (currentGold >= amount)
        {
            UpdateGold(-amount);
            Debug.Log($"감소 골드 : {amount} 현재 골드 : {currentGold}");
        }
        else
        {
            // UI 팝업창 띄우기?
            Debug.Log("골드가 부족합니다...");
        }
    }

    public void UpdateGold(int amount)
    {
        DataManager.Instance.curData.playerData.gold += amount;
        OnGoldChanged?.Invoke(currentGold);
    }
}