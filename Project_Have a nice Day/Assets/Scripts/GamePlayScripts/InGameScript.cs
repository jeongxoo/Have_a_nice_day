using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScript : MonoBehaviour
{
    public Text LengthText;
    public Text NextNumber;

    public void SetLength(int t1, int t2)
    {
        LengthText.text = t1 + "/" + t2;
    }
}
