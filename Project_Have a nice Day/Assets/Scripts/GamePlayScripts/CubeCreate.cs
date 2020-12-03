using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeCreate : MonoBehaviour
{
    int numberRow;
    int numberCol;

    float startX, startZ;

    int stt = 0;

    public TableUnit[,] listScript;
    public Vector3[,] listPost;

    public GameObject unit;

    int[] newMap;

    int index = GameManager.Instance.gameData.puzzle4Index;
    public List<TableUnit> listDeactive;

    public void CreateTableUnit(int newRow, int newCol)
    {
        if (listScript != null)
        {
            for (int i = 0; i < numberRow; i++)
            {
                for (int j = 0; j < numberCol; j++)
                {
                    listDeactive.Add(listScript[i, j]);
                    listScript[i, j].gameObject.SetActive(false);
                }
            }
        }

        numberRow = newRow;
        numberCol = newCol;

        listScript = new TableUnit[numberRow, numberCol];
        listPost = new Vector3[numberRow, numberCol];

        startX = -numberRow / 2.0f + 0.5f;
        startZ = -numberCol / 2.0f + 0.5f;

        for (int i = 0; i < numberRow; i++)
        {
            for (int j = 0; j < numberCol; j++)
            {
                Vector3 pos = new Vector3(startX, -0.1f, startZ);
                startZ += 1;

                InstantNewUnit(pos, i, j);

            }
            startZ = -numberCol / 2.0f + 0.5f;
            startX += 1;
        }

        PlayManager.Instant.gamePlaying.listScript = listScript;
        ApplyData(numberRow);
        PlayManager.Instant.cam.ChangeView(newRow);
    }

    public void ApplyData(int map)
    {
        
        if(GameManager.Instance.gameData.isTutorial == 1)
        {
            newMap = new int[64] { 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, -1, -1, 0, 0, 0, 0, 0, 4, 0, -1, 0, 8, 0, 9, 0, -1, 0, 0, 0, 0, 0, 0, 0, 5, 0, 0, -1, 6, 0, 7, 0, 0, 0, 0, -1, 0, 0, -1, 10 };
        }
        else
        {
            newMap = Map.getObjectMap(numberCol, index).listMap;

        }


        int count = 0;
        int brick = 0;
        for (int i = 0; i < numberRow; i++)
        {
            for (int j = 0; j < numberCol; j++)
            {
                int index = newMap[count];
                switch (index)
                {
                    case 0:
                        listScript[i, j].SetNumver(0);
                        listScript[i, j].ChangeNormalBox();
                        break;
                    case -1:
                        listScript[i, j].SetNumver(0);
                        listScript[i, j].ChangeBlockBox();
                        brick++;
                        break;
                    default:
                        listScript[i, j].SetNumver(newMap[count]);
                        listScript[i, j].ChangeNormalBox();
                        break;
                }
                count++;
            }
        }
        int totalBox = numberRow * numberCol - brick;
        PlayManager.Instant.gamePlaying.SetValue(totalBox, map);
    }

    GameObject GetBox(Vector3 pos)
    {
        GameObject newObj;
        //newObj = Instantiate(unit, pos, Quaternion.identity) as GameObject;

        if (listDeactive.Count == 0)
        {
            newObj = Instantiate(unit, pos, Quaternion.identity) as GameObject;
            newObj.transform.localScale = new Vector3(GameDefine.pikachuNormalScale, GameDefine.pikachuNormalScale, GameDefine.pikachuYScale);
            newObj.transform.eulerAngles = new Vector3(90, 0, 0);
            newObj.transform.SetParent(transform);
        }
        else
        {
            newObj = listDeactive[listDeactive.Count - 1].gameObject;
            newObj.SetActive(true);
            listDeactive[listDeactive.Count - 1].Reset();
            listDeactive[listDeactive.Count - 1].ChangeNormalBox();
            listDeactive.RemoveAt(listDeactive.Count - 1);
            newObj.transform.position = pos;
        }

        return newObj;
    }

    public void InstantNewUnit(Vector3 pos, int row, int col)
    {
        GameObject obj = GetBox(pos);

        listScript[row, col] = obj.GetComponent<TableUnit>();
        listScript[row, col].index = stt;
        stt++;
        listScript[row, col].SetValue(row, col);

        listPost[row, col] = unit.transform.localPosition;
    }
}
