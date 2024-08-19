using UnityEngine;
using UnityEngine.UI;

public class TutorialSkip : MonoBehaviour
{
    [SerializeField] private GameObject skipBtn;
    [SerializeField] private Button interactionBtn;
    private TutorialController controller;
    private GameObject tutorialController;
    private GameObject harvestArea;
    private Transform childTransform;
    private GameObject harvestAreaParents;

    private void Start()
    {
        tutorialController = GameObject.Find("TutorialController");
        controller = tutorialController.GetComponent<TutorialController>();
        harvestAreaParents = GameObject.Find("HarvestAreaParents");
        if (harvestAreaParents != null)
        {
            childTransform = harvestAreaParents.transform.GetChild(0);
            harvestArea = childTransform.gameObject;
        }
    }

    public void ClickSkipBtn()
    {
        controller.CompletedAllTutorials();
        gameObject.SetActive(false);
        interactionBtn.interactable = true;
        harvestArea.SetActive(false);
        GameManager.Instance.Player.topDownMovement.isMoving = true;
        Destroy(tutorialController.gameObject);
    }
}