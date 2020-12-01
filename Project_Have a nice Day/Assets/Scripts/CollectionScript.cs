using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CollectionScript : MonoBehaviour
{
    public Image page1;

    public List<GameObject> buttons;
    public List<GameObject> locks;

    public Button[] charButtons = new Button[4];
    public Image[] charPanel = new Image[4];
    public Image[] charContent = new Image[4];

    public int charKey;
    public int currentKey;

    public Image CloseupPanel;
    public Image CloseupImage;

    private void Awake()
    {
        charKey = 0;
        currentKey = 0;

        buttons = new List<GameObject>();
        locks = new List<GameObject>();
        
        if (SceneManager.GetActiveScene().name == "Collection")
        {
            foreach(Button button in charButtons)
            {
                button.onClick.AddListener(() => SellectScene(button));
            }
        }
    }

    void Start()
    {
        page1.gameObject.SetActive(true);
        buttons.AddRange(GameObject.FindGameObjectsWithTag("student"));
        locks.AddRange(GameObject.FindGameObjectsWithTag("studentLock"));
        for(int i = 1; i < GameManager.Instance.gameData.StationNumber; i++)
        {
            buttons[i - 1].GetComponent<Button>().interactable = true;
            locks[i - 1].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToMain()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void SellectScene(Button button)
    {
        switch(button.tag)
        {
            case "UniversityStudent":
                charKey = 0;
                break;
            case "OfficeWorker":
                charKey = 1;
                break;
            case "GameDeveloper":
                charKey = 2;
                break;
            case "Soldier":
                charKey = 3;
                break;
        }
        ChangeCharacter();
    }

    public void ChangeCharacter()
    {
        if(currentKey != charKey)
        {
            for(int i = 0; i < 4; i++)
            {
                charPanel[i].gameObject.SetActive(false);
            }
            charPanel[charKey].gameObject.SetActive(true);
            charContent[charKey].GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            currentKey = charKey;
        }
    }

    public void IllustButtonClick()
    {

    }
}
