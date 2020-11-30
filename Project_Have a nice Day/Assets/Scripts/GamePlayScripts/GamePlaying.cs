using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaying : MonoBehaviour
{
    public TableUnit[,] listScript;

    public List<TableUnit> listArrayScript;

    public bool isGameOver;

    public int totalBox;
    public int currentNumber;

    int max;

    public void Reset()
    {
        listArrayScript = new List<TableUnit>();
        isGameOver = false;
        currentNumber = 0;

        if(totalBox != 0)
        {
            MainCanvas.Main.ingameScript.SetLength(0, totalBox);
        }
    }

    public void SetValue(int newTotal, int maxNumber)
    {
        totalBox = newTotal;

        switch(maxNumber)
        {
            case 4:
                max = 4 + 1;
                break;
            case 6:
                max = 6 + 1;
                break;
            default:
                max = 8 + 1;
                break;
        }
    }

    public void TableUnitPress(Vec2 vec, bool isFree, int index)
    {
        if (!isGameOver)
        {
            if (isFree)
            {
                if (CheckNumberOnly(index))
                {
                    FreePress(vec, index);
                }
            }
            else
            {
                SelectPress(vec, index);
            }
        }
    }

    public void FreePress(Vec2 vec, int index)
    {
        if (listArrayScript.Count == 0)
        {
            if (listScript[vec.R, vec.C].tableNumber == 1)
            {
                Vector3 pos = listScript[vec.R, vec.C].transform.position;
                PlayManager.Instant.lineControl.AddPoint(new Vector3(pos.x, 0.8f, pos.z));
                listScript[vec.R, vec.C].ChangeToSelectColor();
                listArrayScript.Add(listScript[vec.R, vec.C]);
                CheckGameOver();
                NextNumber(index);
            }
        }
        else if (CheckRoad(vec, listArrayScript[listArrayScript.Count - 1].GetValue()))
        {
            Vector3 pos = listScript[vec.R, vec.C].transform.position;
            PlayManager.Instant.lineControl.AddPoint(new Vector3(pos.x, 0.8f, pos.z));
            listScript[vec.R, vec.C].ChangeToSelectColor();
            listArrayScript.Add(listScript[vec.R, vec.C]);
            CheckGameOver();
            NextNumber(index);
        }
    }

    void SelectPress(Vec2 vec, int index)
    {
        if (listArrayScript.Count >= 2)
        {
            Vec2 oldVec = listArrayScript[listArrayScript.Count - 2].GetValue();
            if (vec.R == oldVec.R && vec.C == oldVec.C)
            {
                TurnBack(listArrayScript[listArrayScript.Count - 1].tableNumber);
                PlayManager.Instant.lineControl.RemovePoint();
                listArrayScript[listArrayScript.Count - 1].ChangeToNormalColor();
                listArrayScript.RemoveAt(listArrayScript.Count - 1);
            }
        }
    }

    void TurnBack(int index)
    {
        if (currentNumber == index)
        {
            currentNumber = Mathf.Max(0, currentNumber - 1);
        }
    }

    bool CheckNumberOnly(int index)
    {
        if (index == currentNumber + 1 || index == 0)
        {
            return true;
        }
        return false;
    }

    bool CheckRoad(Vec2 v1, Vec2 v2)
    {
        if ((v1.R == v2.R && Mathf.Abs(v1.C - v2.C) == 1) || (v1.C == v2.C && Mathf.Abs(v1.R - v2.R) == 1))
        {
            return true;
        }
        return false;
    }

    void NextNumber(int index)
    {
        if (index == currentNumber + 1)
        {
            currentNumber++;
        }
    }

    void CheckGameOver()
    {
        int current = listArrayScript.Count;
        MainCanvas.Main.ingameScript.SetLength(current, totalBox);
        if (!isGameOver && current == totalBox)
        {
            isGameOver = true;
            Debug.Log("WIN!!");
            MainCanvas.Main.bar.StopTimer();
            if(GameManager.Instance.gameData.isTutorial)
            {
                GameManager.Instance.gameData.isTutorial = false;
            }
            else
            {
                GameManager.Instance.gameData.puzzle4Index += 1;
            }
            GameManager.Instance.SaveGameDataToJson();
            MainCanvas.Main.wonPanel.gameObject.SetActive(true);
        }
    }
}
