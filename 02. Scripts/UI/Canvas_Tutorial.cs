using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Tutorial : PopupUI
{
    [SerializeField] private TextMeshProUGUI tutorialText;
    public Slider tutorialSlider;

    public void SetTutorialMessage(string message)
    {
        tutorialText.text = message;
    }

    public void SetSliderValue(float value)
    {
        tutorialSlider.value = value;
    }
}
