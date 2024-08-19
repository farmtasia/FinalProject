using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementRestriction : MonoBehaviour
{
    [SerializeField] private Vector3 lastValidPosition; // 마지막으로 유효했던 플레이어 위치

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            lastValidPosition = other.transform.position; // 플레이어가 유효한 위치에 들어왔을 때 저장
            Debug.Log("제한 구역 내 플레이어 위치");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = lastValidPosition; // 플레이어가 제한 구역을 벗어나면 마지막 유효 위치로 이동
            Debug.Log("제한 구역에서 벗어남");
        }
    }
}
