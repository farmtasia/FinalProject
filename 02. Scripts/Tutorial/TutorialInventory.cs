using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInventory : MonoBehaviour
{
    [SerializeField] private List<InventoryTutorialBase> inventoryTutorials; // 인벤토리 튜토리얼 리스트

    private InventoryTutorialBase currentTutorial = null;
    private int currentIndex = -1;

    [SerializeField] private TutorialController tutorialController;

    private void Start()
    {
        //tutorialController = FindObjectOfType<TutorialController>();

        //playerInventory = GameManager.Instance.Player.GetComponent<Inventory>();
        //tutorialUI = GetComponent<Canvas_Tutorial>();

        //foreach (var tutorial in inventoryTutorials)
        //{
        //    tutorial.Initialize(playerInventory, tutorialUI);
        //}

        //StartInventoryTutorial();
    }

    public void StartInventoryTutorial()
    {
        SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial != null)
        {
            //currentTutorial.Execute(this);
            // TutorialInventory에서 TutorialController를 전달하는 방법
            TutorialController controller = GetComponent<TutorialController>();
            currentTutorial.Execute(controller);
        }
    }

    public void SetNextTutorial()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        if (currentIndex >= inventoryTutorials.Count - 1)
        {
            Debug.Log("인벤토리 튜토리얼 완료");
            return;
        }

        currentIndex++;
        currentTutorial = inventoryTutorials[currentIndex];
        currentTutorial.Enter();
    }  
}
