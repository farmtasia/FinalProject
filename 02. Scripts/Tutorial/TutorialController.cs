using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<TutorialBase> tutorials;
    [SerializeField] private GameObject RestrictedArea;

    private Canvas_Tutorial tutorialUI;

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;
    public GameObject skipBtn;

    private void Start()
    {
        UIManager.Instance.ShowUI<Canvas_Tutorial>(UIs.Popup);
        UIManager.Instance.HideUI<Canvas_Tutorial>();

        tutorialUI = ConnectedTutorialUI();
        

        // 각 튜토리얼 단계에 플레이어 데이터 전달
        foreach (var tutorial in tutorials)
        {
            tutorial.Initialize(tutorialUI);
        }

        //StartTutorial();
    }

    public void StartTutorial()
    {
        tutorialUI.gameObject.SetActive(true);
        SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        // 현재 튜토리얼의 Exit() 호출
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        // 마지막 튜토리얼을 진행했다면 CompletedAllTutorials() 호출
        //if (currentIndex >= tutorials.Count -1)
        //{
        //    CompletedAllTutorials();
        //    return;
        //}

        // 다음 튜터리얼 과정을 currentTutorial로 등록
        currentIndex++;
        currentTutorial = tutorials[currentIndex];

        // 새로 바뀐 튜토리얼의 Enter()호출
        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;

        Debug.Log("튜토리얼 완료");
        DataManager.Instance.curData.playerData.hasCompletedTutorial = true;

        // 제한 구역 비활성화
        var restrictedArea = GameObject.Find("RestrictedArea");
        if (restrictedArea != null)
        {
            restrictedArea.SetActive(false);  // 제한 구역 오브젝트 비활성화
        }
        restrictedArea.SetActive(false);
        skipBtn = GameObject.Find("Canvas_Main(Clone)/SafeArea/Main_UI/TutorialSkipBtn");
        skipBtn.SetActive(false);
        GameManager.Instance.Player.topDownMovement.isMoving = true;
        tutorialUI.gameObject.SetActive(false);
    }

    public Canvas_Tutorial ConnectedTutorialUI()
    {
        if (DataManager.Instance.CheckUIDictionary("Canvas_Tutorial"))
        {
            Canvas_Tutorial tutorialUI = DataManager.Instance.GetUIInDictionary("Canvas_Tutorial") as Canvas_Tutorial;
            if (tutorialUI != null)
            {
                return tutorialUI;
            }
            else
            {
                Debug.LogError("Canvas_Tutorial 인스턴스를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Canvas_Tutorial이 UI 딕셔너리에 없습니다.");
        }

        return null;
    }
}
