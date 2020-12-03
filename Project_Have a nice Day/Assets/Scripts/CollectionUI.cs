using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : MonoBehaviour
{
    public Canvas ill1;
    public Canvas ill2;
    public Canvas ill3;
    public Canvas ill4;

    public void PopIll1()
    {
        ill1.gameObject.SetActive(true);
    }

    public void PopIll2()
    {
        ill2.gameObject.SetActive(true);
    }

    public void PopIll3()
    {
        ill3.gameObject.SetActive(true);
    }

    public void PopIll4()
    {
        ill4.gameObject.SetActive(true);
    }

    public void Hide()
    {
        ill1.gameObject.SetActive(false);
        ill2.gameObject.SetActive(false);
        ill3.gameObject.SetActive(false);
        ill4.gameObject.SetActive(false);
    }
}
