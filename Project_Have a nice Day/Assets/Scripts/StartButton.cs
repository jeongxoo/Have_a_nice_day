using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    public Image startImage;
    // Start is called before the first frame update

    void Start()
    {
        GameManager.Instance.LoadGameDataFromJson();

        if (GameManager.Instance.gameData.seceret == 1)
        {
            startImage.gameObject.SetActive(true);
        }
        else
        {
            startImage.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
