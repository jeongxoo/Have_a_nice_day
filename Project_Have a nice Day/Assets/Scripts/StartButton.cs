using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    public Button start;
    // Start is called before the first frame update

    void Start()
    {
        GameManager.Instance.LoadGameDataFromJson();

        if (GameManager.Instance.gameData.seceret == 1)
        {
            start.gameObject.SetActive(true);
        }
        else
        {
            start.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
