using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Sound // 음악 재생에 사용될 클래스
{
    public string name; // 곡 이름
    public AudioClip clip; // 곡
}

public class AudioManager : MonoBehaviour
{
    // 싱클톤 패턴
    #region 싱클톤패턴
    static public AudioManager instance; // 싱글톤 패턴을 이용한 오디오 매니저 생성을 위해 클래스 자체를 인스턴스화

    void Awake() // 최초 실행
    {
        if (instance == null) // 인스턴스가 null이라면 (초기화 X)
        {
            instance = this; // 인스턴스에 자기 자신을 초기화
            DontDestroyOnLoad(gameObject); // 최초 1회 생성된 인스턴스를 파괴하지 않고 계속 사용하기 위해
        }
        else // 인스턴스가 null이 아니라면 (초기화 O)
        {
            Destroy(gameObject); // 인스턴스가 존재하기 떄문에 새로 생성되는 인스턴스(오디오 매니저)는 파괴
        }
    }
    #endregion 싱클톤패턴

    public GameData GD;

    // 오디오 재생을 위한 변수들
    #region 오디오 재생을 위한 변수들

    public AudioSource[] audioSourceEffects; // 효과음 > 한번에 여러개가 재생될수 있기때문에 배열
    public AudioSource audioSourceBGM; // 배경음 > 한번에 한곡씩 재생되기때문에 배열 X

    public string playingBGMName; // 현재 재생중인 배경음악의 이름 반환을 위함

    public Sound[] effectSounds;  // 여러 효과음을 담기 위해 배열로
    public Sound[] bgmSounds; // 여러 배경음을 담기 위해 배열로

    [SerializeField]
    private string BGM_Title;

    [SerializeField]
    private string BGM_Main;

    [SerializeField]
    private string BGM_Play;

    [SerializeField]
    private string Button1;

    [SerializeField]
    private string Button2;

    [SerializeField]
    private string Button3;

    #endregion 오디오 재생을 위한 변수들

    // 효과음용 함수들
    #region 효과음용 함수들
    // 효과음 재생을 위한 함수
    public void PlayEffectSound(string _name) // sound 클래스의 name과 일치여부를 판단하기 위해 _name을 입력받음
    {
        for (int i = 0; i < effectSounds.Length; i++) // 보유한 효과음들 중에 아래 조건을 만족하는게 있는지 파악하기 위한 반복문
        {
            if (_name == effectSounds[i].name) // 입력받은 이름이 보유한 효과음 목록에 존재하는지 판단
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if (!audioSourceEffects[j].isPlaying) // 재생중인 소스가 없다면 참, 있다면 거짓
                    {
                        //playingSoundName[j] = effectSounds[j].name;
                        Debug.Log("start play se");
                        audioSourceEffects[j].clip = effectSounds[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                    else
                    {
                        audioSourceEffects[j + 1].clip = effectSounds[i].clip;
                        audioSourceEffects[j + 1].Play();
                        return;
                    }
                }
            }
        }
    }

    public void PlayButton1()
    {
        PlayEffectSound(Button1);
    }

    public void PlayButton2()
    {
        PlayEffectSound(Button2);
    }

    public void PlayButton3()
    {
        PlayEffectSound(Button3);
    }

    #endregion 효과음용 함수들

    // BGM용 함수들
    #region BGM용 함수들

    // 배경음을 재생하기 위한 함수
    public void PlayBGM(string _name) // 위와 마찬가지로 이름을 입력받음
    {
        for (int i = 0; i < bgmSounds.Length; i++) // 효과음과 동일
        {
            if (bgmSounds[i].name == _name) // 입력받은 이름이 보유한 배경음 목록에 존재하는지 판단
            {
                if (audioSourceBGM.isPlaying) // 노래중에 재생중인 노래가 있는지 판단 / 있으면 참, 없으면 거짓
                {
                    if (playingBGMName == _name) // 재생중인 노래가 있다면 재생중인 노래의 이름과 입력한 노래의 이름을 비교 / 같으면 참, 다르면 거짓
                    {
                        Debug.Log("same song is playing"); // 두 제목이 같다면 같은노래를 재생중이라는 문구 출력 후 종료
                        Debug.Log("Song name : " + playingBGMName);
                        return;
                    }
                    else
                    {
                        Debug.Log("change song"); // 두 제목이 다르다면 재생중이었던 노래를 종료후 새로 입력받은 이름에 해당하는 노래를 재생을 시작
                        playingBGMName = bgmSounds[i].name;
                        Debug.Log("Song name : " + playingBGMName);
                        audioSourceBGM.Stop();
                        audioSourceBGM.clip = bgmSounds[i].clip;
                        audioSourceBGM.Play();
                        return;
                    }

                    //그러면 노래가 재생중인게 있으면 그게 내가 입력한 이름의 것과 같으면 그냥 두고
                    // 같지 않다면 재생중인 곡을 중지하고 내가 입력한것을 재생하게
                }
                else // 아무 음악도 재생중이지 않다면 입력받은 이름을 가진 오디오 클립을 재생
                {
                    Debug.Log("start playing song");
                    playingBGMName = bgmSounds[i].name;
                    Debug.Log("Song name : " + playingBGMName);
                    audioSourceBGM.clip = bgmSounds[i].clip;
                    audioSourceBGM.Play();
                    return;
                }
            }
        }
    }

    // 브금 컨트롤용 함수
    public void PlayBGMWhatYouWant(string _sceneName)
    {
        if(_sceneName == "Title")
        {
            PlayBGM(BGM_Title);
        }
        else if(_sceneName == "MainGameScene")
        {
            PlayBGM(BGM_Main);
        }
        else if(_sceneName == "GamePlayScene")
        {
            PlayBGM(BGM_Play);
        }
    }

    #endregion BGM용 함수들



    // 슬라이더를 만들고 슬라이더의 벨류값을 오디오소스의 볼륨 값에 넣는i
}

/* 간단한 흐름 정리
 * 1. 효과음
 * 효과음의 경우 한번에 여러가지 효과음 (ex. 버튼 클릭, 게임 클리어 등) 이 출력되는 경우도 존재
 * 그렇기 때문에 오디오 소스 변수를 배열로 선언
 * 
 * 2. 배경음
 * 배경음의 경우 여러 종류의 배경음이 존재하기는 하나 한번에 하나의 배경음만 출력되게 되므로
 * 효과음과 다르게 배열이 아닌 일반 변수로 오디오 소스를 선언
 * 배경음 재생 함수를 통해 원하는 배경음의 이름을 전달 받고
 * > 보유한 배경음들 중 동일한 이름을 가진 배경음이 있는지 판단
 * > 있다면 현재 재생중인 배경음이 있는지 판단
 * > 있다면 재생중인 음악과 새로 재생하려는 음악의 이름이 같은지 판단 / 재생중인 음악이 없다면 입력받은 이름을 가진 배경음을 재생 후 종료
 * > 이름이 서로 같다면 같은 노래를 재생중이라는 로그와 함께 종료 / 다른 이름이라면 기존의 노래를 종료 후 새로 입력받은 노래를 재생
 * 
 */

