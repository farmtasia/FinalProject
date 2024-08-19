using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{ 
    [SerializeField] private CanvasGroup loadingScreen;    // 로딩 창
    private string preSceneName;
    private string loadSceneName;

    // TODO
    // 1. 씬 로드되고 전달해야 할 데이터가 있다면 그 데이터까지 다 이동한 뒤에 페이드 인
    // 2. 진행도 바 대신 로딩 애니메이션 추가

    public void LoadScene(string _preSceneName, string _loadSceneName)
    {
        Time.timeScale = 0f;
        transform.GetChild(0).gameObject.SetActive(true);    // TODO: 다른 창 또 생기면 인덱스 번호 추가
        SceneManager.sceneLoaded += LoadSceneEnd;
        preSceneName = _preSceneName;
        loadSceneName = _loadSceneName;
        UIManager.Instance.ClearSortingOrder();
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        yield return StartCoroutine(Fade(true));    // 이전에 하던 작업을 다 끝내고 페이드 아웃
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = true;    // 로딩 끝나고 자동으로 씬 전환
    }

    private void LoadSceneEnd(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == loadSceneName)
        {
            if (preSceneName != SceneName.StartScene.ToString())
            {
                MovePosition();
                GameManager.Instance.SaveData();
            }
            GameManager.Instance.Player.GetComponentInChildren<SpriteRenderer>().enabled = true;
            StartCoroutine(Fade(false));
            Time.timeScale = 1f;
            SceneManager.sceneLoaded -= LoadSceneEnd;
        }
    }

    private IEnumerator Fade(bool isFadeOut)
    {
        yield return new WaitForSecondsRealtime(0.1f);    // TODO: 데이터 로드가 다 끝나면 이동하도록 수정
        float duration = 1f;
        float timer = 0f;

        while (timer <= duration)    // 1초 동안 페이드 아웃
        {
            timer += Time.unscaledDeltaTime * 2f;    // 어두워지는 속도 2배
            loadingScreen.alpha = Mathf.Lerp(isFadeOut ? 0 : 1, isFadeOut ? 1 : 0, timer / duration);    // 페이드 인이라면 투명도가 1에서 0(보임), 페이드 아웃이라면 투명도가 0에서 1(안 보임)
            yield return null;
        }

        //loadingScreen.alpha = isFadeOut ? 1 : 0;

        if (!isFadeOut)    // 페이드 인이라면 밝아지고 나서 해당 오브젝트 비활성화
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if (preSceneName == SceneName.StartScene.ToString())
            {
                GameManager.Instance.Player.ApplyData();
            }
        }
    }

    // TODO: 작동 확인
    public void MovePosition()
    {
        if (preSceneName == "FarmScene")
            GameManager.Instance.Player.transform.position = new Vector2(-23.55f, 16.5f);
        else if (preSceneName == "SeaScene")
            GameManager.Instance.Player.transform.position = new Vector2(-4.5f, -20.5f);
        else if (loadSceneName == "FarmScene")
            GameManager.Instance.Player.transform.position = new Vector2(-16.3f, -3.2f);
        else if (loadSceneName == "SeaScene")
            GameManager.Instance.Player.transform.position = new Vector2(0.5f, -26f);

        //string positionObjTag = "";
        //// 마을 씬으로 이동
        //if (preSceneName == SceneName.FarmScene.ToString())
        //    positionObjTag = preSceneName;
        //else if (preSceneName == SceneName.SeaScene.ToString())
        //    positionObjTag = preSceneName;
        //// 마을 씬에서 이동
        //else if (loadSceneName == SceneName.FarmScene.ToString())
        //    positionObjTag = loadSceneName;
        //else if (loadSceneName == SceneName.SeaScene.ToString())
        //    positionObjTag = loadSceneName;

        //GameObject moveToObj = GameObject.FindWithTag(positionObjTag);
        //GameManager.Instance.Player.transform.position = moveToObj.transform.position;
    }
}
