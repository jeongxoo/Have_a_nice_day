using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public string currentSceneName; // 현재 씬의 이름을 저장하기위함

    public void Awake() // start로 노래를 틀기전에 먼저 현재 씬의 이름을 반환받음
    {
        currentSceneName = SceneManager.GetActiveScene().name; // 현재씬의 이름을 저장
        //Debug.Log("You are in the " + currentSceneName + " now ");
    }

    public void Start()
    {
        AudioManager.instance.PlayBGMWhatYouWant(currentSceneName); // 현재씬의 이름을 바탕으로 알맞는 노래를 재생
    }

    // To Main
    #region To_Main
    public void AnyToMain() // 출발지 : 어디서나 도착지 : 메인 씬
    { 
        SceneManager.LoadScene("MainGameScene");
        AudioManager.instance.PlayButton1();
    }
    #endregion To_main

    // From Main
    #region From_Main
    public void MainToPlay() // 출발지 : 메인 씬 도착지 : 플레이 씬
    {
        SceneManager.LoadScene("GamePlayScene");
        AudioManager.instance.PlayButton2();

    }

    public void MainToCollect() // 출발지 : 메인 씬 도착지 : 수집 씬
    {
        SceneManager.LoadScene("Collection");
        AudioManager.instance.PlayButton1();
    }

    public void MaintToSetting() // 출발지 : 메인 씬 도착지 : 설정 씬
    {
        SceneManager.LoadScene("SettingScene");
        AudioManager.instance.PlayButton1();
    }

    public void ToGame()
    {
        SceneManager.LoadScene("GamePlay");
        AudioManager.instance.PlayButton2();
    }

    #endregion From_Main

    // InGame
    #region In_Game

    public void RestartGame()   // 출발지 : 게임플레이 도착지 : 게임플레이
    {
        SceneManager.LoadScene("GamePlayScene");
        AudioManager.instance.PlayButton2();
    }

    #endregion In_Game

    public void TitleToMain()
    {
        SceneManager.LoadScene("MainGameScene");
        AudioManager.instance.PlayButton1();
        Map.LoadMap4();
        Map.LoadMap6();
        Map.LoadMap8();
    }

    public void FirstTitleToMain()
    {
        GameManager.Instance.ResetGameData2();
        SceneManager.LoadScene("MainGameScene");
        AudioManager.instance.PlayButton1();
        Map.LoadMap4();
        Map.LoadMap6();
        Map.LoadMap8();
    }
}


