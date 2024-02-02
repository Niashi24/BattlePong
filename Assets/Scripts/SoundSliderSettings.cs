using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSliderSettings : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private string saveName = "SavedMasterVolume";

    [SerializeField]
    private string propertyName = "MasterVolume";

    private void Start()
    {
        SetVolume(PlayerPrefs.GetFloat(saveName, 100));
    }

    public void SetVolume(float _value)
    {
        if (_value < 1f)
            _value = 0.001f;
        
        RefreshSlider(_value);
        PlayerPrefs.SetFloat(saveName, _value);
        mixer.SetFloat(propertyName, Mathf.Log10(_value / 100f) * 20f);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(slider.value);
    }

    public void RefreshSlider(float _value)
    {
        slider.value = _value;
    }
}
