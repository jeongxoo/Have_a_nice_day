using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TableUnit : MonoBehaviour
{
    public int tableNumber;
    public int index;

    int row, col;

    public Color selectColor, normalColor;
    public Material faceMat;
    public MeshRenderer faceRender;
    public GameObject faceObj;
    public GameObject blockObj;
    public TextMesh text;

    public Color[] listColor;

    public enum State
    {
        Free,
        Select,
        Block
    }

    public State state = State.Free;

    private void Awake()
    {
        faceMat = faceRender.material;
    }

    public bool IsFree()
    {
        if (state == State.Free)
        {
            return true;
        }
        return false;
    }

    public bool IsSelect()
    {
        if (state == State.Select)
        {
            return true;
        }
        return false;
    }

    public void ChangeBlockBox()
    {
        state = State.Block;
        faceObj.SetActive(false);
        blockObj.SetActive(true);
    }

    public void ChangeNormalBox()
    {
        faceObj.SetActive(true);
        blockObj.SetActive(false);
    }

    public Vec2 GetValue()
    {
        return new Vec2(row, col);
    }

    private void OnMouseOver()
    {
        //Debug.Log(tableNumber);
        PlayManager.Instant.gamePlaying.TableUnitPress(new Vec2(row, col), IsFree(), tableNumber);
    }

    public void SetNumver(int num)
    {
        tableNumber = num;
        if (num == 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
            text.text = num.ToString();
            text.color = listColor[num - 1];
        }
    }

    public void SetValue(int r, int c)
    {
        row = r;
        col = c;
    }

    public void ChangeToSelectColor()
    {
        state = State.Select;
        faceMat.SetColor("_Color", selectColor);
    }

    public void ChangeToNormalColor()
    {
        state = State.Free;
        faceMat.SetColor("_Color", normalColor);
    }
}
