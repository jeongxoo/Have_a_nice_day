using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUnit : MonoBehaviour
{
    public int map = 4;
    public int index = 1;

    public void Button()
    {
        PlayManager.Instant.startGameScript.StartPlayLevel(map, index);
        PlayManager.Instant.gamePlaying.Reset();
        PlayManager.Instant.lineControl.Reset();
    }
}
