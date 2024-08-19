using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IInteractable
{
    public string GetInteractPrompt(); // 화면에 띄어줄 프롬프트
    public void OnInteract(); // 인터랙트 됐을 때 어떤 효과를 발생시킬건지.
}

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f; // 0.05초마다 레이를 쏴서 체크(호출간격시간)
    private float lastCheckTime; // 마지막으로 체크한 시간
    public float maxCheckDistance; // 레이 최대 거리
    public LayerMask layerMask; // 어떤 레이어가 달려있는 게임오브젝트를 추출할건지
    public Vector2 boxSize = new Vector2(1f, 1f); // 레이 박스의 크기

    public GameObject curInteractGameObject; // 인터랙션에 성공했다면, 현재 인터랙션된 게임오브젝트 정보를 가져올 것
    public IInteractable curInteractable; // 인터페이스 캐싱

    //public TextMeshProUGUI promptText; // 프롬프트에 띄어줄 텍스트 -> 나중에 UI 쪽으로 분리?
    private Camera camera; // 이름바꿔줘야되나....

    //public GameObject interactionUI; // Active 이미지

    public bool otherCase;

    // 모든 기본 방향과 대각선 방향을 포함하는 벡터들(레이)
    //private Vector2[] directions = {
    //    Vector2.up, Vector2.down, Vector2.left, Vector2.right, // 기본 방향
        //new Vector2(1, 1).normalized, new Vector2(1, -1).normalized, // 대각선 방향
        //new Vector2(-1, 1).normalized, new Vector2(-1, -1).normalized // 대각선 방향
    //};

    private GameObject highlightedObject; // 하이라이트 된 오브젝트 저장

    private TopDownMovement topDownMovement;

    void Start()
    {
        camera = Camera.main; // 메인 카메라 연결
        topDownMovement = GetComponent<TopDownMovement>();
        otherCase = true;    // 스타트 씬에서 초기화 에러 방지
        //Canvas_Main.interactionUI.SetActive(false);

    }

    void Update()
    {
        if (otherCase) return; // 레이캐스트를 임시적으로 막아주는 것
        
        if (Time.time - lastCheckTime > checkRate) // (현재 시간 - 마지막체크시간)이 호출간격보다 크다면 
        {
            lastCheckTime = Time.time;
            CheckForInteractable();
        }
    }

    private void CheckForInteractable()
    {
        curInteractGameObject = null;
        curInteractable = null;
        if (Canvas_Main.promptText == null || Canvas_Main.interactionUI == null) return;
        Canvas_Main.promptText.gameObject.SetActive(false);
        Canvas_Main.interactionUI.SetActive(false);

        if (highlightedObject != null) // 이전에 하이라이트된 오브젝트가 있다면 초기화
        {
            var highlightScript = highlightedObject.GetComponent<Highlight>();
            if (highlightScript != null)
            {
                highlightScript.OnMouseExit();
            }
            highlightedObject = null;
        }

        Vector2 direction = topDownMovement.GetMovementDirection().normalized; // 플레이어가 바라보는 방향
        Vector2 boxCenter;

        if (direction != Vector2.zero)
        {
            // 플레이어가 움직이고 있는 경우: 이동 방향에 따라 박스 중심 설정
            boxCenter = (Vector2)transform.position + direction * (maxCheckDistance / 2);
        }
        else
        {
            // 플레이어가 멈춘 경우: 마지막 이동 방향을 사용하여 박스 중심 설정
            boxCenter = (Vector2)transform.position + topDownMovement.GetMovementDirection().normalized * (maxCheckDistance / 2);
        }

        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, direction, maxCheckDistance, layerMask);
        Debug.DrawRay(transform.position, direction * maxCheckDistance, Color.red);

        if (hit.collider != null)
        {
            curInteractGameObject = hit.collider.gameObject;
            curInteractable = hit.collider.GetComponent<IInteractable>();

            if (curInteractable != null)
            {
                SetPromptText();
                Canvas_Main.interactionUI.SetActive(true);
                HighlightedObject(hit.collider.gameObject);
            }
        }

        //foreach (var direction in directions)
        //{
        //    Vector2 boxCenter = (Vector2)transform.position + direction * (maxCheckDistance / 2); // 박스 중심

        //    RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, direction, maxCheckDistance, layerMask);
        //    Debug.DrawRay(transform.position, direction * maxCheckDistance, Color.red);

        //    //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxCheckDistance, layerMask);
        //    //Debug.DrawRay(transform.position, direction * maxCheckDistance, Color.red);

        //    if (hit.collider != null)
        //    {
        //        curInteractGameObject = hit.collider.gameObject;
        //        curInteractable = hit.collider.GetComponent<IInteractable>();

        //        if (curInteractable != null)
        //        {
        //            SetPromptText();
        //            Canvas_Main.interactionUI.SetActive(true);
        //            HighlightedObject(hit.collider.gameObject);                  
        //        }
        //    }
        //}
    }

    private void SetPromptText()
    {

        if (curInteractable != null)
        {
            Canvas_Main.promptText.gameObject.SetActive(true); // 프롬프트창(UI) 띄어주고
            Canvas_Main.promptText.text = curInteractable.GetInteractPrompt(); // 해당 오브젝트의 프롬프트내용(텍스트) 실행
        }
        else
        {
            Canvas_Main.promptText.gameObject.SetActive(false);
        }
    }

    private void HighlightedObject(GameObject obj)
    {
        var highlightScript = obj.GetComponent<Highlight>();
        if (highlightScript != null)
        {
            highlightScript.OnMouseEnter();
            highlightedObject = obj; // 하이라이트된 오브젝트 저장
        }
    }

    public void OnItemInteract()
    {           
        if (curInteractGameObject != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            Canvas_Main.promptText.gameObject.SetActive(false);
            // TODO(다빈) : 아이템 획득 알림창 재설정해야 함
            Canvas_Main.interactionUI.SetActive(false);
        }
    }

    public void UseEquipTool()
    {
        // 해당 도구에 맞게 메서드가 동작할 수 있도록 함
        GameManager.Instance.Player.equipment.curEquipTool.UseTool();
    }

    public void OnEquipUI(bool isOn)
    {
        Canvas_Main.interactionUI.SetActive(isOn);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            Vector2 direction = topDownMovement.GetMovementDirection().normalized; // 플레이어가 바라보는 방향
            Vector2 boxCenter = (Vector2)transform.position + direction * (maxCheckDistance / 2);
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}