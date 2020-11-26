using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    int currentMap;
    int currentIndex;

    public void StartPlayLevel(int map, int index)
    {
        currentMap = map;
        currentIndex = index;

        Midle();
    }

    public void Midle()
    {
        PlayManager.Instant.cubeCreate.CreateTableUnit(currentMap, currentMap);
    }
}
