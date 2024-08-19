using UnityEngine;
using UnityEngine.UI;

public class EquipTool : ItemObjectBase
{
    public Player player;
    public Button interactionBtn;

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    public virtual void UseTool()
    {
        // 도구가 작동될 코드를 작성해서 넣어야 함
    }

    protected virtual void ChangeInteractButtonFunction()
    {
        player.interaction.otherCase = true;
        interactionBtn.onClick.RemoveAllListeners();
        interactionBtn.onClick.AddListener(player.interaction.UseEquipTool);
    }

    protected virtual void ResetInteractButtonFunction()
    {
        player.interaction.otherCase = false;
        interactionBtn.onClick.RemoveAllListeners();
        interactionBtn.onClick.AddListener(player.interaction.OnItemInteract);
    }
}
