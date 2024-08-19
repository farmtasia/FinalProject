using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{   
    public ItemSlot dragItemSlot;
    public Image itemImage;
    public float ImageAlphaValue = 1f;

    public void DragSetImage(Image itemImg)
    {
        itemImage.sprite = itemImg.sprite;
        SetColor(ImageAlphaValue); // 드래그 중인 아이템 이미지의 알파값을 조절
    }

    public void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
}
