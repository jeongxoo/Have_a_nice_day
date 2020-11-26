using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    static PlayManager main;
    public GamePlaying gamePlaying;
    public LineControl lineControl;
    public CubeCreate cubeCreate;
    public StartGameScript startGameScript;
    public ItemUnit itemUnit;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;

        main = this;
    }

    public static PlayManager Instant
    {
        get { return main; }
    }

    private void Start()
    {
        itemUnit.Button();
    }
}
