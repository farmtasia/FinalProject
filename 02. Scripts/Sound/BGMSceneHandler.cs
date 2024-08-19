using UnityEngine;

public class BGMSceneHandler : MonoBehaviour
{
    public BGMType bgmType;

    private void Start()
    {
        // 씬에 있는 SoundManager에서 BGM을 재생
        SoundManager.Instance.PlayBGM(bgmType);
    }
}
