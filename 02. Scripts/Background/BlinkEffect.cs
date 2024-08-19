using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BlinkEffect : MonoBehaviour
{
    [Range(0.1f, 1f)]
    public float speed;
    float time;
    private Image sr;

    private void Start()
    {
        sr = GetComponent<Image>();
    }

    void Update()
    {
        time += Time.unscaledDeltaTime;

        // 주기를 조정하여 알파 값이 더 천천히 변하도록 설정
        float alpha = (Mathf.Sin(time * Mathf.PI * speed) + 1) * 0.5f;

        // 알파 값을 적용하여 스프라이트 색상 변경
        sr.color = new Color(1, 1, 1, alpha);
    }
}
