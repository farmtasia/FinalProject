using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeletePopUpUI : MonoBehaviour
{
    // 팝업 창은 하나만 있음
    // 데이터 삭제 버튼을 클릭하면 삭제 팝업이 뜸(버튼에 해당 팝업 온오프 연결)
    // 팝업에는 이름이 적용되고 확인 누르면 해당 데이터 슬롯의 삭제 메서드 호출

    [SerializeField] private TextMeshProUGUI sentence;    // 팝업 문구
    [SerializeField] private Button deleteBtn;
    [SerializeField] private Button cancelBtn;
    private LoadDataSlot loadDataSlot;

    private void Start()
    {
        deleteBtn.onClick.AddListener(DeleteButton);
        cancelBtn.onClick.AddListener(CancelButton);
    }

    public void SetPopUp(LoadDataSlot obj, CharacterData data)
    {
        loadDataSlot = obj;
        sentence.text = $"'{data.nameData.userName}'(을)를 정말로 삭제하시겠습니까?";
    }
    
    public void DeleteButton()
    {
        loadDataSlot.DeleteLoadDataSlot();
        gameObject.SetActive(false);
    }

    public void CancelButton()
    {
       gameObject.SetActive(false);
    }
}
