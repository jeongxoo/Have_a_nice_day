using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{

    public Button reset;

    private void Awake()
    {
        //GameManager.Instance.LoadGameDataFromJson();

    }

    private void Start()
    {
        if (GameManager.Instance.gameData.seceret == 1)
        {
            reset.gameObject.SetActive(true);
        }
        else
        {
            reset.gameObject.SetActive(false);
        }
    }

}
