using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TilemapToSprite : MonoBehaviour
{
    public Camera tilemapCamera;
    public int width = 1024;
    public int height = 1024;
    public string savePath = "Assets/screenshot.png";

    void Start()
    {
        StartCoroutine(CaptureTilemap());
    }

    IEnumerator CaptureTilemap()
    {
        // RenderTexture와 Texture2D를 알파 채널을 포함하여 설정
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        tilemapCamera.targetTexture = renderTexture;

        // 카메라의 배경색을 투명하게 설정
        tilemapCamera.backgroundColor = new Color(0, 0, 0, 0);
        tilemapCamera.clearFlags = CameraClearFlags.SolidColor;

        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

        yield return new WaitForEndOfFrame();

        tilemapCamera.Render();
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tilemapCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(savePath, bytes);

        Debug.Log("Screenshot saved at: " + savePath);

        // 캡처한 텍스처를 스프라이트로 변환
        Sprite sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(0.5f, 0.5f));

        // 저장된 스프라이트를 사용할 오브젝트에 적용 (예: SpriteRenderer)
        GameObject spriteObject = new GameObject("CapturedTilemap");
        SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }
}
