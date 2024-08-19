using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{

    public string Speaker { get; private set; } // 말하는 사람
    public string DialogueText { get; private set; } // 대화 내용

    public DialogueType Type { get; private set; } // 대화 타입

    public string[] Choices { get; private set; } // 선택지 배열

    public Dialogue(string speaker, string dialogueText, DialogueType dialogueType, string[] choices = null)
    {
        Speaker = speaker;
        DialogueText = dialogueText;
        Type = dialogueType;
        Choices = choices;
    }

}
