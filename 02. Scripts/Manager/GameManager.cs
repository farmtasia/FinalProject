using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private Player player; // 실제 Player 인스턴스를 저장하는 변수

    protected override void Awake()
    {
        base.Awake();
    }

    public Player Player // Player 프로퍼티-> GameManager에서 관리하는 Player 인스턴스
    {
        get
        { 
            if (player == null)
                player = FindObjectOfType<Player>();

                if (player == null)
                {
                    GameObject obj = new GameObject(typeof(Player).Name, typeof(Player));
                    player = obj.GetComponent<Player>();
                }

            return player;
        }
        set { player = value; }
    }

    public void SaveData()   // 현재 캐릭터 데이터 저장
    {
        DataManager.Instance.CurDataUpdate();
        ResourcesManager.Instance.SaveData();
    }

    public void SaveDataAndExit()   // 현재 캐릭터 데이터 저장 및 종료
    {
        DataManager.Instance.CurDataUpdate();
        ResourcesManager.Instance.SaveData();

        // 전처리기 사용
        #if UNITY_EDITOR    // 유니티 에디터라면 실행 중지
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();     // 빌드 상태라면 게임 종료
        #endif
    }

    public void Exit()    // 스타트 씬의 종료 기능
    {
        #if UNITY_EDITOR    // 유니티 에디터라면 실행 중지
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();     // 빌드 상태라면 게임 종료
        #endif
    }
}