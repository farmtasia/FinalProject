using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public int mySortingOrder;

    public void SetOrder(int order)
    {
        mySortingOrder = order;
        GetComponent<Canvas>().sortingOrder = order;
    }

}
