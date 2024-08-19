using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : MonoBehaviour
{
    public GameObject seed; // 씨앗 오브젝트
    public Animator seedAnimator; // 씨앗 애니메이터
    public GameObject growthObj; // 작물 성장과정이 담긴 오브젝트
    public ItemSO seedSO; // 심을 씨앗 아이템
}
