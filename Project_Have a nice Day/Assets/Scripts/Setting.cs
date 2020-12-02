using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField]
    private Slider bgmVolumeSlider;

    [SerializeField]
    private Slider effectVolumeSlider;

    public void Start()
    {
        GameManager.Instance.LoadGameDataFromJson();
        bgmVolumeSlider.value = GameManager.Instance.gameData.bgmVol;
        effectVolumeSlider.value = GameManager.Instance.gameData.effectVol;
    }

    public void BGMVolume()
    {
        GameManager.Instance.BgmVolumeControll(bgmVolumeSlider.value);
    }

    public void EffectVolume()
    {
        GameManager.Instance.EffectVolumeControll(effectVolumeSlider.value);
    }
}
