using UnityEngine;
using System.Collections;

public class BgScale : MonoBehaviour
{

    public void SetBg()
    {
        float height = 2f * PlayManager.Instant.cam.cam.orthographicSize;
        float width = height * PlayManager.Instant.cam.cam.aspect;
        transform.localScale = new Vector3(width, height, 1);
    }
}
