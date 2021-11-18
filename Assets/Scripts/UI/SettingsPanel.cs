using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    private const float SliderMaxValue = 10;
    private const float RatioToVolume = 1 / SliderMaxValue;

    [Header("UI")] 
    [SerializeField] private Text bgmVolumeText;
    [SerializeField] private Text sfxVolumeText;
    
    public void OnBGMSliderValueChanged(float value)
    {
        bgmVolumeText.text = value.ToString(CultureInfo.InvariantCulture);
        AudioManager.Instance.BGMVolume = value * RatioToVolume;
    }
    
    public void OnSFXSliderValueChanged(float value)
    {
        sfxVolumeText.text = value.ToString(CultureInfo.InvariantCulture);
        AudioManager.Instance.SFXVolume = value * RatioToVolume;
    }

    public void OnClickTitleButton()
    {
        SceneManagerEx.Instance.LoadScene(SceneType.Title);
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
