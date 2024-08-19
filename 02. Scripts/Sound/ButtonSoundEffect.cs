using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private SEType clickSoundEffect = SEType.CLICK; // 클릭 시 효과음

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(() => SoundManager.Instance.PlayEffect(clickSoundEffect));
    }
}
