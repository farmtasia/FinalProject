using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObjectPool : MonoBehaviour
{
    public ObjectPoolManager objectPoolManager;
    //private ObjectPoolManager.Pool poolInfo;
    public AudioSource audioSource { get; private set; }
    public string soundName;

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
        //poolInfo = objectPoolManager.GetPool(soundName);
        audioSource = GetAudioSource();
    }

    // 오디오 소스 컴포넌트
    private AudioSource GetAudioSource()
    {
        GameObject obj = objectPoolManager.poolDictionary[soundName].Peek();
        
        return obj.GetComponent<AudioSource>();
    }
    
    // 직접 실행 및 중지는 조건에 맞춰 사용하는 스크립트에 작성
    public void GetSound(string soundName)
    {
        objectPoolManager.SpawnFromPool(soundName);

        StartCoroutine(ReturnSound(gameObject, soundName));
    }

    public IEnumerator ReturnSound(GameObject gameObject, string soundName)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        objectPoolManager.ReturnToPool(gameObject, soundName);
    }
}
