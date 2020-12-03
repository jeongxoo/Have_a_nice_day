using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnit : MonoBehaviour
{
    public int map;
    public int index = 1;

    private void Awake()
    {
        GameManager.Instance.LoadGameDataFromJson();
        map = GameManager.Instance.gameData.knn;
    }

    public void Button()
    {
        PlayManager.Instant.startGameScript.StartPlayLevel(map, index);
        PlayManager.Instant.gamePlaying.Reset();
        PlayManager.Instant.lineControl.Reset();
    }
}
