using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Range(0.1f, 100)]
    public float speed = 10;
    //public Transform[] backgrounds;
    public RectTransform[] backgrounds;
    private Vector3 rightpos;
    private Vector3 leftpos;
    private int curCnt = 0;
    float leftPosX = 0f;
    float rightPosX = 0f;
    float xScreenHalfSize;
    float yScreenHalfSize;

    void Start()
    {
        leftpos = backgrounds[0].position;
        leftpos.x -= 960;
        rightpos = backgrounds[2].position;
        rightpos.x += 960;
        // yScreenHalfSize = Camera.main.orthographicSize;
        // xScreenHalfSize = yScreenHalfSize * Camera.main.aspect;
        //
        // leftPosX = -(xScreenHalfSize * 2);
        // rightPosX = xScreenHalfSize * 2 * backgrounds.Length;
    }

    void Update()
    {
        backgrounds[0].position -= new Vector3(10, 0, 0) * (Time.unscaledDeltaTime * speed);
        backgrounds[1].position -= new Vector3(10, 0, 0) * (Time.unscaledDeltaTime * speed);
        backgrounds[2].position -= new Vector3(10, 0, 0) * (Time.unscaledDeltaTime * speed);

        if (backgrounds[curCnt].position.x <= leftpos.x)
        {
            backgrounds[curCnt].position = rightpos;
            curCnt++;
            if (curCnt > 2)
                curCnt = 0;
        }

        // for (int i = 0; i < backgrounds.Length; i++)
        // {
        //     backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.unscaledDeltaTime;
        //
        //     if (backgrounds[i].position.x < leftPosX)
        //     {
        //         Vector3 nextPos = backgrounds[i].position;
        //         nextPos = new Vector3(nextPos.x + rightPosX, nextPos.y, nextPos.z);
        //         backgrounds[i].position = nextPos;
        //     }
        // }
    }
}
