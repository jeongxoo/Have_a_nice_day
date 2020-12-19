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

    public int preparedEpisode;
    public int preparedStage;

    // 게임매니저
    #region 싱글톤(게임매니저)
    private void Awake() // 이거는 게임매니저 싱글톤 코드
    {

        var objs = FindObjectsOfType<GameManager>();
        if(objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        Time.timeScale = 1;

        instance = this;
    }

    public static GameManager Instance // 이거는 외부에서 게임매니저를 참조할 수 있게 해주는 Instance 생성 코드
    {
        get
        {
            return instance;
        }
    }


    #endregion 싱글톤(게임매니저)

    private void Start()
    {
        LoadGameDataFromJson();

        BgmVolumeControll(gameData.bgmVol);
        EffectVolumeControll(gameData.effectVol);

        preparedEpisode = 4;
        preparedStage = 9;
    }

    // JSON 데이터 save&load
    #region JSON

    // 데이터 저장 함수
    [ContextMenu("To Json Data")]
    public void SaveGameDataToJson() // 현재 GameData를 JSON파일로 저장해주는 함수
    {
        string jsonData = JsonUtility.ToJson(gameData, true);
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        File.WriteAllText(path, jsonData);
    }

    // 데이터 로드 함수
    [ContextMenu("From Json Data")]
    public void LoadGameDataFromJson() // JSON파일의 정보를 현재 GameData로 불러오는 함수
    {
        string path = Path.Combine(Application.persistentDataPath, "GameData.json");
        string jsonData = File.ReadAllText(path);
        gameData = JsonUtility.FromJson<GameData>(jsonData);
    }

    #endregion JSON


    #region 현재 게임 스테이지 정보
    public void GetScore()
    {
        LoadGameDataFromJson();

        if (gameData.StationNumber <= preparedEpisode && gameData.StageNumber < preparedStage) // Station은 4이하, Stage는 9미만이면 Stage만 1 증가하고 저장!!
        {
            gameData.StageNumber += 1;
            gameData.puzzle4Index += 1;
            SaveGameDataToJson();

            Debug.Log(" 다음 스테이지로 이동합니다. 현재 " + gameData.StationNumber
                + " 번째 역의 " + gameData.StageNumber + "스테이지 입니다."); 
        }
        else if (gameData.StationNumber <= preparedEpisode && gameData.StageNumber == preparedStage) 
        {
            if (gameData.StationNumber == preparedEpisode) // 현재 준비된 에피소드는 4개
            {
                Debug.Log("Waiting for next update!");
                gameData.StationNumber += 1;
                gameData.StageNumber = 1;
                SaveGameDataToJson();
                return;
            }
            else // 게임 클리어가 아니라면 Station은 1 증가하고 Stage는 1로 초기화!!
            {
                gameData.StageNumber = 1;
                gameData.StationNumber += 1;
                gameData.puzzle4Index += 1;
                SaveGameDataToJson();

                Debug.Log(" 모든 스테이지 클리어 다음 역인 " + gameData.StationNumber
                    + " 번째 역의 " + gameData.StageNumber + "스테이지로 이동합니다.");
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
    }

    public void EffectVolumeControll(float _value) // Effect 볼륨 조절
    {
        gameData.effectVol = _value;
        for (int i = 0; i < audioManager.audioSourceEffects.Length; i++)
        {
            audioManager.audioSourceEffects[i].volume = gameData.effectVol;
            SaveGameDataToJson();
        }
    }

    public void ResetGameData() // Station이랑 Stage초기화 할 때 쓰는 함수~
    {
        gameData.StageNumber = 0;
        gameData.StationNumber = 1;
        gameData.puzzle4Index = 0;
        gameData.bgmVol = 1;
        gameData.effectVol = 1;
        gameData.isTutorial = 1;
        gameData.numberOfRenew = 0;
        gameData.updatedBestLabelInGame = 0;
        gameData.knn = 4;
        gameData.seceret = 0;
        SaveGameDataToJson();
    }

    public void ResetGameData2() // Station이랑 Stage초기화 할 때 쓰는 함수~
    {
        gameData.StageNumber = 0;
        gameData.StationNumber = 1;
        gameData.puzzle4Index = 0;
        gameData.bgmVol = 1;
        gameData.effectVol = 1;
        gameData.isTutorial = 1;
        gameData.numberOfRenew = 0;
        gameData.updatedBestLabelInGame = 0;
        gameData.knn = 4;
        gameData.seceret = 1;
        SaveGameDataToJson();
    }
}

[System.Serializable]
public class GameData // 게임 데이타 저장용 클래스 
{
    public int StageNumber = 0; 
    public int StationNumber = 1; 
    public int puzzle4Index = 0;
    public float bgmVol = 1;
    public float effectVol = 1;
    public int isTutorial = 1;
    public int numberOfRenew = 0;
    public float clearTime = 0.0f;
    public float updatedBestLabelInGame = 0;
    public int knn = 4;
    public int seceret = 1;
}