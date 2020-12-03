using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bar : MonoBehaviour
{
    public ItemUnit itemUnit;

    public Image image;
    float duration;

    IEnumerator timeRun;
    bool runing;

    private void Start()
    {
        if(GameManager.Instance.gameData.isTutorial)
        {
            TimerGo(8);
        }
        else
        {
            TimerGo(itemUnit.map);
        }
    }

    public void TimerGo(int map)
    {
        //float totalDuration;

        switch (map)
        {
            case 4:
                duration = 40;
                break;
            case 6:
                duration = 60;
                break;
            default:
                duration = 80;
                break;

        }

        StopTimer();

        timeRun = StartTimer();
        StartCoroutine(timeRun);
    }

    public void StopTimer()
    {
        if (runing)
        {
            StopCoroutine(timeRun);
            runing = false;
        }
    }

    IEnumerator StartTimer()
    {
        runing = true;
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            image.fillAmount = 1 - timer / duration;
            yield return null;
        }

        MainCanvas.Main.Lose();
        runing = false;
    }

}
