using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    static MainCanvas mainCanvas;
    public InGameScript ingameScript;
    public Bar bar;

    public Image lostPanel;
    public Image wonPanel;
    public Text renewTest;
    public Text clearTimeTest;

    public Canvas unLock;

    public Sprite[] ill;
    public Image image;

    public Text station;
    public Text stage;

    public Text knnText;

    private void Awake()
    {
        mainCanvas = this;
    }

    private void Start()
    {
        PrintStageData();
    }

    public static MainCanvas Main
    {
        get { return mainCanvas; }
    }

    public void UnLockILL()
    {
        if (GameManager.Instance.gameData.StageNumber == 9)
        {
            switch (GameManager.Instance.gameData.StationNumber)
            {   
                case 1:
                    unLock.gameObject.SetActive(true);
                    image.sprite = ill[0];
                    break;

                case 2:
                    image.sprite = ill[1];
                    unLock.gameObject.SetActive(true);
                    break;

                case 3:
                    image.sprite = ill[2];
                    unLock.gameObject.SetActive(true);
                    break;

                case 4:
                    image.sprite = ill[3];
                    unLock.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public void Lose()
    {
        PlayManager.Instant.gamePlaying.isGameOver = true;
        Debug.Log("LOSE!!");
        lostPanel.gameObject.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void GoHome()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void RenewTest()
    {
        renewTest.GetComponent<Text>().text = "재시작 : " + GameManager.Instance.gameData.numberOfRenew;
        clearTimeTest.GetComponent<Text>().text = "클리어 시간 : " + GameManager.Instance.gameData.clearTime;
    }

    public void PrintStageData()
    {
        if (GameManager.Instance.gameData.StageNumber == 0)
        {
            station.GetComponent<Text>().text = "Tutorial";
            stage.GetComponent<Text>().text = "숫자 1에서 시작! \n" + "가장 큰 숫자로 연결! \n"
                +"모든 빈칸을 채우면 승리! \n" +  "잘못 갔을 경우 뒤로 가거나 \n" + "재시작 버튼 사용! \n"
                + "내장된 머신러닝 모델이 \n" + "플레이 결과를 통해 \n" + "당신에게 적합한 난이도를 추천해줍니다.";

        }
        else
        {
            station.GetComponent<Text>().text = "Episode : " + GameManager.Instance.gameData.StationNumber;
            stage.GetComponent<Text>().text = "Stage : " + GameManager.Instance.gameData.StageNumber + "/9";
        }
    }

    public void PrintKNN()
    {
        GameManager.Instance.LoadGameDataFromJson();

        //Debug.Log(GameManager.Instance.gameData.knn);
        if (GameManager.Instance.gameData.StageNumber > 1 && GameManager.Instance.gameData.StageNumber <= 9)
        {
            knnText.GetComponent<Text>().text = "실력 측정 중...";
        }
        else
        {
            switch (GameManager.Instance.gameData.knn)
            {
                case 4:
                    knnText.GetComponent<Text>().text = "추천 난이도 설정 EASY";
                    break;

                case 6:
                    knnText.GetComponent<Text>().text = "추천 난이도 설정 NORMAL";
                    break;

                case 8:
                    knnText.GetComponent<Text>().text = "추천 난이도 설정 HARD";
                    break;
            }
        }
    }
}
