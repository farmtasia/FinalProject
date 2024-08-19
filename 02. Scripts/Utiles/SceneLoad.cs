using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    string preSceneName;

    private void OnTriggerEnter2D()
    {
        // 통로 입구에 오브젝트 추가해놓고 감지하면 해당 씬 이동
        string loadSceneName = gameObject.name;

        // 감지된 레이어 이름이 씬일 때만 이동
        if (loadSceneName == SceneName.FarmScene.ToString() || loadSceneName == SceneName.VillageScene.ToString() || loadSceneName == SceneName.SeaScene.ToString())
            preSceneName = SceneManager.GetActiveScene().name;
            SceneLoadManager.Instance.LoadScene(preSceneName, loadSceneName);
    }
}
