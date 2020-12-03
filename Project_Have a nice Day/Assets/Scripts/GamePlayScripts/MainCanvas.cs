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

    private void Awake()
    {
        mainCanvas = this;
    }

    private void Start()
    {
        station.GetComponent<Text>().text = "Episode : " + GameManager.Instance.gameData.StationNumber;
        stage.GetComponent<Text>().text = "Stage : " + GameManager.Instance.gameData.StageNumber + "/9";
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
        if (GameManager.Instance.gameData.isTutorial)
        {
            renewTest.GetComponent<Text>().text = "재시작 : " + GameManager.Instance.gameData.numberOfRenew;
            clearTimeTest.GetComponent<Text>().text = "클리어 시간 : " + GameManager.Instance.gameData.clearTime;

        }
    }
}
