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

    private void Awake()
    {
        mainCanvas = this;
    }

    public static MainCanvas Main
    {
        get { return mainCanvas; }
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
        GameManager.Instance.LoadGameDataFromJson();

        if (GameManager.Instance.gameData.isTutorial)
        {
            renewTest.GetComponent<Text>().text = "재시작 : " + GameManager.Instance.gameData.numberOfRenew;
            clearTimeTest.GetComponent<Text>().text = "클리어 시간 : " + GameManager.Instance.gameData.clearTime;

        }
    }
    

}
