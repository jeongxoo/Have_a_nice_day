using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class CollectionScripts : MonoBehaviour
{
    public GameObject image;

    public Image[] character = new Image[4];

    public Button[] buttons = new Button[4];

    public Image[] content = new Image[4];

    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 pos3;
    private Vector3 pos4;

    public TextMesh text;

    private static int key;

    public CharacterIllust characterillust;

    public Image PopUp;

    private void Awake()
    {
        key = 0;

        LoadGameDataFromJson();
        if (SceneManager.GetActiveScene().name == "Collection")
        {
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() => SellectScene(button)); // 현재 누른 버튼을 참조하고 그 버튼이 참조한 함수 실행
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        int num = (int)gameObject.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < 4; i++)
        {
            character[i].transform.Translate((num * 2) * i, 0, 0);
        }
        pos1 = image.transform.position;
        pos2 = new Vector3(pos1.x - (num * 2), pos1.y, 0);
        pos3 = new Vector3(pos1.x - (num * 4), pos1.y, 0);
        pos4 = new Vector3(pos1.x - (num * 6), pos1.y, 0);

        if (characterillust.isSetting == false)
        {
            characterillust.studentButtons = GameObject.FindGameObjectsWithTag("student");
            characterillust.workerButtons = GameObject.FindGameObjectsWithTag("worker");
            characterillust.developerButtons = GameObject.FindGameObjectsWithTag("developer");
            characterillust.soldierButtons = GameObject.FindGameObjectsWithTag("soldier");
            characterillust.isSetting = true;
            SaveGameDataToJson();
        }

        for (int i = 0; i < characterillust.studentNumber; i++)
            characterillust.studentButtons[i].GetComponent<Button>().interactable = true;
        for (int i = 0; i < characterillust.workerNumber; i++)
            characterillust.workerButtons[i].GetComponent<Button>().interactable = true;
        for (int i = 0; i < characterillust.developerNumber; i++)
            characterillust.developerButtons[i].GetComponent<Button>().interactable = true;
        for (int i = 0; i < characterillust.soldierNumber; i++)
            characterillust.soldierButtons[i].GetComponent<Button>().interactable = true;
    }

    public void SellectScene(Button button)
    {
        switch (button.tag)
        {
            case "UniversityStudent":
                key = 0;
                MoveCollectionPanel(pos1);
                break;
            case "OfficeWorker":
                key = 1;
                MoveCollectionPanel(pos2);
                break;
            case "GameDeveloper":
                key = 2;
                MoveCollectionPanel(pos3);
                break;
            case "Soldier":
                key = 3;
                MoveCollectionPanel(pos4);
                break;
        }
    }

    public void MoveCollectionPanel(Vector3 vec)
    {
        image.transform.position = vec;
        for(int i = 0; i < 4; i++)
        {
            if (key == i)
                continue;
            content[i].GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        }
    }

    public void StageClearButton()
    {
        int accumulateNumber = characterillust.AccumulateNumber % 4;
        switch (accumulateNumber)
        {
            case 1:
                characterillust.studentButtons[characterillust.studentNumber].GetComponent<Button>().interactable = true;
                characterillust.studentNumber++;
                characterillust.AccumulateNumber += 1;
                SaveGameDataToJson();
                break;
            case 2:
                characterillust.workerButtons[characterillust.workerNumber].GetComponent<Button>().interactable = true;
                characterillust.workerNumber++;
                characterillust.AccumulateNumber += 1;
                SaveGameDataToJson();
                break;
            case 3:
                characterillust.developerButtons[characterillust.developerNumber].GetComponent<Button>().interactable = true;
                characterillust.developerNumber++;
                characterillust.AccumulateNumber += 1;
                SaveGameDataToJson();
                break;
            case 0:
                characterillust.soldierButtons[characterillust.soldierNumber].GetComponent<Button>().interactable = true;
                characterillust.soldierNumber++;
                characterillust.AccumulateNumber += 1;
                SaveGameDataToJson();
                break;
        }
    }

    public void ToMain()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void SaveGameDataToJson()
    {
        string jsonData = JsonUtility.ToJson(characterillust, true);
        string path = Path.Combine(Application.dataPath, "CollectionData.json");
        File.WriteAllText(path, jsonData);
    }

    public void LoadGameDataFromJson()
    {
        string path = Path.Combine(Application.dataPath, "CollectionData.json");
        string jsonData = File.ReadAllText(path);
        characterillust = JsonUtility.FromJson<CharacterIllust>(jsonData);
    }

    public void ImageMagnification()
    {
        PopUp.GetComponent<Image>().gameObject.SetActive(true);
    }

    public void ImageShrinkage()
    {
        PopUp.GetComponent<Image>().gameObject.SetActive(false);
    }
}

[System.Serializable]
public class CharacterIllust
{
    public bool isSetting = false;

    public int AccumulateNumber = 1;
    public int studentNumber = 0;
    public int workerNumber = 0;
    public int developerNumber = 0;
    public int soldierNumber = 0;

    public GameObject[] studentButtons;
    public GameObject[] workerButtons;
    public GameObject[] developerButtons;
    public GameObject[] soldierButtons;
}