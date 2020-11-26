using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Illustration
{
    public Sprite illBlack;
    public Sprite[] illColor;
    //public string name;
    //public int number;
}

public class Main_Illurstration : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer intoBlack;

    [SerializeField]
    private SpriteRenderer[] intoColor;

    [SerializeField]
    private Illustration[] illustrations;

    [SerializeField]
    private int checkPoint;

    public void LoadIllustration()
    {
        checkPoint = GameManager.Instance.currentStageNumber;

        for (int i = 0; i < illustrations.Length; i++)
        {
            if (GameManager.Instance.currentStationNumber - 1 == i)
            {
                intoBlack.sprite = illustrations[i].illBlack;

                for (int j = 0; j < intoColor.Length; j++)
                {
                    intoColor[j].sprite = illustrations[i].illColor[j];
                    intoColor[j].gameObject.SetActive(false);

                    if(GameManager.Instance.currentStageNumber - 1 == j)
                    {
                        for (int a = 0; a < checkPoint; a++)
                        {
                            intoColor[a].gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }
}
//지금 해볼만한거는 포문으로 다 넣어주고 일단 전부다 셋액티브 펄스로 바꾸고 1번에서 누르면 1개 2번에서 누르면 2개 활성화
