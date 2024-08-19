using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteBox2 : MonoBehaviour, IInteractable
{
    public GameObject blackBox2; // BlackBox2 오브젝트
    public Equipment equipment;
    public EquipTool curTool;

    [SerializeField] private Sprite[] groundStates; // 잔디, 일반 땅, 파인 땅 이미지 배열
    [SerializeField] private GameObject[] seedPrefabs; // 씨앗 애니메이션 프리팹 배열
    private SpriteRenderer spriteRenderer; // 자식 오브젝트
    private int currentStateIndex = 0; // 현재 상태 인덱스


    private void Awake()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>(); // 자식오브젝트 중 0번째를 가져온다
    }

    private void Start()
    {
        equipment = GameManager.Instance.Player.equipment;

        if (groundStates.Length > 0)
        {
            spriteRenderer.sprite = groundStates[0]; // 초기 상태 이미지 설정
        }
    }

    void UpdateStage() // 땅파기 이미지 업데이트
    {
        if (currentStateIndex >= 0 && currentStateIndex <= groundStates.Length)
        {
            spriteRenderer.sprite = groundStates[currentStateIndex];
        }
    }

    public string GetInteractPrompt() // 오브젝트가 감지됐을때 뜨는 프롬프트 문구
    {
        return "화이트박스 감지";
    }

    public void OnInteract() // 인터랙트 됐을 때 일어나는 동작
    {
        curTool = equipment.curEquipTool;

        if (curTool != null && curTool is Shovel)
        {
            Debug.Log("땅파기. 삽질");
            currentStateIndex++;

            if (currentStateIndex >= groundStates.Length)
            {
                currentStateIndex = groundStates.Length - 1; // 마지막 상태 유지
                StartCoroutine(PlantSeedAndActivateBlackBox());
            }
            else
            {
                UpdateStage();
            }
        }
        else
        {
            gameObject.SetActive(true);
            blackBox2.SetActive(false);
        }
    }

    private IEnumerator PlantSeedAndActivateBlackBox()
    {
        PlantSeed();
        yield return new WaitForSeconds(1); // 애니메이션 재생 시간만큼 대기
        gameObject.SetActive(false); // 나자신 비활성화
        blackBox2.SetActive(true); // 블랙박스 활성화
        ResetGround();
    }

    private void PlantSeed()
    {
        int randomIndex = Random.Range(0, seedPrefabs.Length);

        // 프리팹이 null인지 확인
        if (seedPrefabs[randomIndex] == null)
        {
            return;
        }

        GameObject seed = Instantiate(seedPrefabs[randomIndex], transform); // seed오브젝트를 WhiteBox오브젝트 자식으로 생성
        seed.transform.localPosition = Vector3.zero; // seed의 위치를 0,0,0으로 만들어서 부모의 위치를 기준으로 설정되도록.

        Destroy(seed, 1); // 1초 후 애니메이션 오브젝트 삭제
    }

    private void ResetGround()
    {
        // 땅파기 상태 초기화
        currentStateIndex = 0;
        UpdateStage();
    }
}