using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider bgmSlider;

    void Start()
    {
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        bgmSlider.value = AudioManager.Instance.GetBGMVolume();
    }

    public void UpdateSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }

    public void UpdateBGMVolume()
    {
        AudioManager.Instance.SetBGMVolume(bgmSlider.value);
    }

}
