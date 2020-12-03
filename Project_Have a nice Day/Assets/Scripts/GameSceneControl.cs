using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneControl : MonoBehaviour
{
    private GamePlay tile_root = null;

    void Start()
    {
        this.tile_root = this.gameObject.GetComponent<GamePlay>();  //게임플레이 스크립트 가져오고

        this.tile_root.initialSetUp();          //타일만들고 배치하는 메서드 호출
    }

    void Update()
    {
        
    }
}
