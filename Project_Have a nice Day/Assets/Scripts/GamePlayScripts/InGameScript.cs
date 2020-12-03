using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScript : MonoBehaviour
{
    public Text LengthText;
    public Text NextNumber;

    public int renewNumber = 0;
    public float cleartime = 0;

    public Button Renew;
    public Button TutorialRenew;

    private void Awake()
    {
        if(GameManager.Instance.gameData.isTutorial == 1)
        {
            TutorialRenew.gameObject.SetActive(true);
        }
        else
        {
            TutorialRenew.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        cleartime += Time.deltaTime;
    }

    public void SetLength(int t1, int t2)
    {
        LengthText.text = t1 + "/" + t2;
    }

    public void ReNew()
    {
        PlayManager.Instant.lineControl.Reset();
        PlayManager.Instant.gamePlaying.Reset();
    }

    public void TutorialReNew()
    {
        PlayManager.Instant.lineControl.Reset();
        PlayManager.Instant.gamePlaying.Reset();
        renewNumber += 1;
    }

    public void SetNumberNext(int number, int max, bool isWin)
    {
        if (number == 1)
        {
            NextNumber.text = "Start at number " + number;
        }
        else if (number == max)
        {
            if (isWin)
            {
                SetNumberNextDone();
            }
            else
            {
                NextNumber.text = "Must go through all the boxes";
            }
        }
        else
        {
            NextNumber.text = "Go to number " + number;
        }
    }

    public void SetNumberNextDone()
    {
        NextNumber.text = "Done !";
    }
}
