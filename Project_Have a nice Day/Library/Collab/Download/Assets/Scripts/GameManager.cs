using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public GameData gameData;
    public AudioManager audioManager;

    // Station이 Stage보다 상위 개념!!
    public int currentStationNumber; // 현재 역
    public int currentStageNumber; // 현재 스테이지      
    public int currentPuzzle4Index; // 퍼즐인덱스!! 아마 1~200까지 그리고 이거는 예시라서 4 by 4만 적용됨

    public float saveBgmVol; // BGM 볼륨
    public float saveEffectVol; // Effect 볼륨

    // 게임매니저
    #region 싱글톤(게임매니저)
    public static GameManager Instance // 이거는 외부에서 게임매니저를 참조할 수 있게 해주는 Instance 생성 코드
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newobj = new GameObject().AddComponent<GameManager>();
                    instance = newobj;
                }
            }
            return instance;
        }
    }

    private void Awake() // 이거는 게임매니저 싱글톤 코드
    {
        var objs = FindObjectsOfType<GameManager>();
        if(objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion 싱글톤(게임매니저)

    private void Start()
    {
        LoadGameDataFromJson();
        currentStageNumber = gameData.StageNumber;
        currentStationNumber = gameData.StationNumber;
        currentPuzzle4Index = gameData.puzzle4Index;
        saveBgmVol = gameData.bgmVol;
        saveEffectVol = gameData.effectVol;
    }

    // JSON 데이터 save&load
    #region JSON

    // 데이터 저장 함수
    public void SaveGameDataToJson() // 현재 GameData를 JSON파일로 저장해주는 함수
    {
        string jsonData = JsonUtility.ToJson(gameData, true);
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, jsonData);
    }

    // 데이터 로드 함수
    public void LoadGameDataFromJson() // JSON파일의 정보를 현재 GameData로 불러오는 함수
    {
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        string jsonData = File.ReadAllText(path);
        gameData = JsonUtility.FromJson<GameData>(jsonData);
    }

    #endregion JSON


    // 임시로 사용할 역과 스테이지 관련 변수와 함수들
    #region 현재 게임 스테이지 정보를 담은 임시용


    public void GetScore()
    {
        if (gameData.StationNumber <= 10 && gameData.StageNumber < 4) // Station은 10이하, Stage는 4이하면 Stage만 1 증가하고 저장!!
        {
            gameData.StageNumber += 1;
            gameData.puzzle4Index += 1;
            currentStageNumber = gameData.StageNumber;
            currentPuzzle4Index = gameData.puzzle4Index;
            SaveGameDataToJson();

            Debug.Log(" 다음 스테이지로 이동합니다. 현재 " + gameData.StationNumber
                + " 번째 역의 " + gameData.StageNumber + "스테이지 입니다."); // 출력한번 해주고
            //LoadGameDataFromJson();
        }
        else if (gameData.StationNumber <= 10 && gameData.StageNumber == 4) // Station 10이하, Stage == 4일때
        {
            if (gameData.StationNumber == 10) // Station이 10이면 게임 완전히 클리어
            {
                Debug.Log("ALL STATION CLEAR!!");
                return;
            }
            else // 게임 클리어가 아니라면 Station은 1 증가하고 Stage는 1로 초기화!!
            {
                gameData.StageNumber = 1;
                gameData.StationNumber += 1;
                gameData.puzzle4Index += 1;
                currentStageNumber = gameData.StageNumber;
                currentStationNumber = gameData.StationNumber;
                currentPuzzle4Index = gameData.puzzle4Index;
                SaveGameDataToJson();

                Debug.Log(" 모든 스테이지 클리어 다음 역인 " + gameData.StationNumber
                    + " 번째 역의 " + gameData.StageNumber + "스테이지로 이동합니다."); // 출력한번 해주고
                //LoadGameDataFromJson();
            }
        }
    }
    #endregion 현재 게임 스테이지 정보를 담은 임시용

    //볼륨 조절
    public void BgmVolumeControll(float _value) // BGM 볼륨 조절
    {
        gameData.bgmVol = _value;
        audioManager.audioSourceBGM.volume = gameData.bgmVol;
        SaveGameDataToJson();
        saveBgmVol = gameData.bgmVol;
    }

    public void EffectVolumeControll(float _value) // Effect 볼륨 조절
    {
        gameData.effectVol = _value;
        for (int i = 0; i < audioManager.audioSourceEffects.Length; i++)
        {
            audioManager.audioSourceEffects[i].volume = gameData.effectVol;
            SaveGameDataToJson();
        }
        saveEffectVol = gameData.effectVol;
    }

    public void ResetGameData() // Station이랑 Stage초기화 할 때 쓰는 함수~
    {
        gameData.StationNumber = 1;
        gameData.StageNumber = 1;
        gameData.puzzle4Index = 0;
        SaveGameDataToJson();
    }
}

[System.Serializable]
public class GameData // 게임 데이타 저장용 클래스 (추후에 수정)
{
    public int StageNumber = 1; // 스테이지는 1 부터 4까지
    public int StationNumber = 1; // 스테이션은 10까지만
    public int puzzle4Index = 0;
    public int[] CharacterNumber = new int[4];
    public int[] IllustrationNumber = new int[4];
    public int[] NumberOfIllustration = new int[10];

    public float bgmVol = 1;
    public float effectVol = 1;
}