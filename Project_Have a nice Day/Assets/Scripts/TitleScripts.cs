using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScripts : MonoBehaviour
{
    public Image subway;
    public Image title;
    public Image gameLogo;
    public Image startButton;
    public Image bridge;
    public Image train;
    public Image reset;

    private float easing = 7.0f;
    private float s_easing = 70.0f;
    float time = 0;
    float F_time1 = 3.0f;
    float F_time2 = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(SmoothMove(title.transform.position, this.transform.position
            - new Vector3(0,-650,0), easing, title));
        //bridge.transform.position = new Vector3(950, 180, 0);
        StartCoroutine(FixBridge(bridge.transform.position, bridge.transform.position
            - new Vector3(10, -1525, 0), easing, bridge));
        GameManager.Instance.LoadGameDataFromJson();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds, Image obj)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            obj.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }        
        StartCoroutine(FadeFlow(gameLogo, F_time1));
        StartCoroutine(MoveSubway(subway.transform.position, subway.transform.position - new Vector3(-4000,0,0), s_easing, subway));
        StartCoroutine(MoveTrain(train.transform.position, train.transform.position - new Vector3(1000, 0, 0), easing, train));
    }

    IEnumerator FixBridge(Vector3 startPos, Vector3 stopPos, float seconds, Image birdge)
    {
        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime / seconds;
            bridge.transform.position = Vector3.Lerp(startPos, stopPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        yield return null;
    }

    IEnumerator FadeFlow(Image panel, float F_time)
    {
        Color alpha = panel.color;
        time = 0;
        while(alpha.a < 1)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            panel.color = alpha;
            yield return null;
        }
        yield return null;

        startButton.gameObject.SetActive(true);
        StartCoroutine(FadeFlow(startButton, F_time2));

    }

    IEnumerator MoveSubway(Vector3 startPos, Vector3 stopPos, float seconds, Image sub)
    {
        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime / seconds;
            sub.transform.position = Vector3.Lerp(startPos, stopPos, t);
            yield return null;
        }
        yield return null;
    }

    IEnumerator MoveTrain(Vector3 startPos, Vector3 stopPos, float seconds, Image train)
    {
        float t = 0;
        while (t < 1.0f)
        {
            t += Time.deltaTime / seconds;
            train.transform.position = Vector3.Lerp(startPos, stopPos, t);
            yield return null;
        }
        yield return null;
    }
}
