using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    // BGM과 효과음을 각각 재생하기 위해 배열 AudioSource 사용
    private AudioSource[] audioSources = new AudioSource[(int)Sound.MAX];
    // 오디오 클립 캐싱을 위한 딕셔너리. 경로를 키로 하고 AudioClip을 값으로 저장
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        base.Awake();

        // Sound 열거형을 사용하여 여러 오디오 소스를 관리
        for (int i = 0; i < audioSources.Length; i++)
        {
            GameObject go = new GameObject { name = ((Sound)i).ToString() }; // 각 오디오 소스를 관리할 새로운 GameObject 생성
            audioSources[i] = go.AddComponent<AudioSource>(); // 생성한 GameObject에 AudioSource 컴포넌트를 추가
            go.transform.parent = this.transform; // GameObject를 SoundManager의 자식으로 설정
        }

        // BGM은 루프를 기본으로 설정
        audioSources[(int)Sound.BGM].loop = true;
    }


    public void Clear() // 기존 재생하던 음악 모두 멈추고 캐싱해둔 오디오 클립 모두 삭제
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
        audioClips.Clear();
    }


    // 오디오 소스 볼륨 설정
    public void SetVolume(Sound type, float volume)
    {
        audioSources[(int)type].volume = volume;
    }
    
    // 오디오 소스 볼륨 가져오기
    public float GetVolume(Sound type)
    {
        return audioSources[(int)type].volume;
    }


    public bool PlayBGM(BGMType bgmType, float pitch = 1.0f)
    {
        // BGM 경로
        string path = $"Sound/BGM/{bgmType}";
        // Play 메서드를 호출하여 BGM 재생
        return Play(Sound.BGM, path, pitch);
    }


    public bool PlayEffect(SEType seType, float pitch = 1.0f)
    {
        // 효과음 경로
        string path = $"Sound/Effect/{seType}";
        // Play 메서드를 호출하여 효과음 재생
        return Play(Sound.EFFECT, path, pitch);
    }


    // 오디오 소스 유형과 경로를 사용하여 오디오 클립을 재생
    public bool Play(Sound type, string path, float pitch)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        AudioSource audioSource = audioSources[(int)type]; // 지정된 오디오 소스 가져오기
        audioSource.pitch = pitch; // 피치 설정

        // BGM은 Clip 재생
        if (type == Sound.BGM)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(path); // 오디오 클립 로드
            if (audioClip == null)
            {
                return false;
            }

            if (audioSource.isPlaying) // 이미 재생 중이면 정지
            {
                audioSource.Stop();
            }
            // 오디오 소스에 클립 할당 및 재생
            audioSource.clip = audioClip;
            audioSource.Play();
            return true;
        }
        // 효과음은 PlayOneShot 재생
        else if (type == Sound.EFFECT)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
            {
                return false;
            }

            audioSource.PlayOneShot(audioClip);
            return true;
        }
        return false;
    }

    // 경로를 통해 오디오 클립을 가져오고 캐싱하는 메서드
    private AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;
        if (audioClips.TryGetValue(path, out audioClip)) // 딕셔너리에서 값을 찾은 경우(이미 메모리에 로드되어 있음.
                                                         // 이전에 Resources.Load를 통해 로드된 것이며, 딕셔너리에 저장된 상태.
                                                         // -> 이미 캐싱된 클립)
        {
            return audioClip; // 바로 반환
        }

        // 캐싱되지 않은 경우
        audioClip = Resources.Load<AudioClip>(path); // 경로에 해당하는 오디오 클립 로드
        audioClips.Add(path, audioClip); // 새로 로드한 오디오클립을 딕셔너리에 추가
        return audioClip; // 반환
    }
}




// 기존
/*

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// TODO: Enums.cs로 옮기기
public enum SoundType
{
    BGM,
    SFX
}

// TODO: 작동되는지 추후 확인 후 수정 필요
public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixer audioMixer;    // 여러 타입의 사운드 조절
    private SoundObjectPool soundObjectPool;

    // TODO: 저장 및 불러오기 기능 추가
    private float curBGMVolume, curSFXVolume = 0.5f;    // 현재 배경음악, 효과음 볼륨

    [SerializeField] private Slider BGMSlider;
    [SerializeField] private Slider SFXSlider;

    //private AudioSource audioSource;
    
    //private Dictionary<string, AudioClip> clipsDictionary;    // 클립들을 담을 딕셔너리

    //[SerializeField] private AudioClip[] preLoadClips;    // 사용할 클립들 사전 로드

    //private List<TemporarySoundPlayer> instantiatedSounds;    // 루프 재생 오디오 제거용



    private void Start()
    {
        //audioSource = soundObjectPool.audioSource;
        BGMSlider.value = curBGMVolume;
        SFXSlider.value = curSFXVolume;
        
        BGMSlider.onValueChanged.AddListener(SetBGMVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);


        //InitVolumes(curBGMVolume, curSFXVolume);
        // --- X
        //clipsDictionary = new Dictionary<string, AudioClip>();
        
        //foreach (AudioClip clip in preLoadClips)    // 딕셔너리에 오디오 클립 추가
        //    clipsDictionary.Add(clip.name, clip);

        //instantiatedSounds = new List<TemporarySoundPlayer>();
    }


    // 오디오 소스에서 볼륨 조절(초기값, 중간 조절값) - 믹서와 연결해서 전체 소리 한 번에 조절
    // curVolume과 볼륨 연결
    // UI 슬라이더와도 연결
    //public void InitVolumes(float bgm, float sfx)    // 초기 볼륨 설정
    //{
    //    SetVolume(SoundType.BGM, bgm);
    //    SetVolume(SoundType.SFX, sfx);
    //}

    //private void SetVolume(SoundType type, float value)    // 볼륨 조절
    //{
    //    audioMixer.SetFloat(type.ToString(), Mathf.Log10(value) * 20);
    //}

    private void SetBGMVolume(float value)
    {
        audioMixer.SetFloat(SoundType.BGM.ToString(), Mathf.Log10(value) * 20);
    }
    
    private void SetSFXVolume(float value)
    {
        audioMixer.SetFloat(SoundType.SFX.ToString(), Mathf.Log10(value) * 20);
    }

    // 만약 루프 체크가 되어있다면 암것도 안함(배경음)
    // 루프 체크가 없다면(효과음) 오브젝트 풀로 관리
    // 직접 실행 및 중지는 조건에 맞춰 사용하는 스크립트에 작성

    public void PlaySound(string soundName, SoundType type = SoundType.SFX)
    {
        soundName = soundObjectPool.soundName;


        if (soundObjectPool.audioSource.loop)
            return;
        else
        {
            //soundObjectPool.audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(type.ToString())[0];    // 어차피 하나니까 없어도 상관 없나
            soundObjectPool.GetSound(soundName);
        }
    }









    //private AudioClip GetClip(string clipName)    // 딕셔너리에서 파일 이름으로 클립 탐색 후 반환
    //{
    //    AudioClip clip = clipsDictionary[clipName];

    //    if (clip == null)    // 예외 처리
    //        Debug.LogError(clipName + "이 없습니다.");

    //    return clip;
    //}

    //private void AddToList(TemporarySoundPlayer soundPlayer)    // 루프 형태로 재생된 경우에는 나중에 제거하기 위해 리스트에 저장
    //{
    //    instantiatedSounds.Add(soundPlayer);
    //}

    //public void StopLoopSound(string clipName)    // 이름으로 루프 사운드 찾아서 제거
    //{
    //    foreach (TemporarySoundPlayer soundPlayer in instantiatedSounds)
    //    {
    //        if  (soundPlayer.clipName == clipName)
    //        {
    //            instantiatedSounds.Remove(soundPlayer);
    //            Destroy(soundPlayer.gameObject);
    //            return;
    //        }
    //    }
    //    Debug.LogWarning(clipName + "이 없습니다.");
    //}

    //public void PlaySound1(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.SFX)
    //{
    //    GameObject obj = new GameObject("TemporarySoundPlayer");
    //    TemporarySoundPlayer soundPlayer = obj.AddComponent<TemporarySoundPlayer>();

    //    if (isLoop)    // 루프 사운드는 리스트에 저장
    //        AddToList(soundPlayer);

    //    soundPlayer.InitSound(GetClip(clipName));
    //    soundPlayer.Play(audioMixer.FindMatchingGroups(type.ToString())[0], delay, isLoop);
    //}
}
*/