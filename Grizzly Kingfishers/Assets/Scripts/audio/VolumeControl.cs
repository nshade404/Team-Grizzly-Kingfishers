using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;

public class VolumeControl : MonoBehaviour
{
    public const string MASTER_VOLUME = "MasterVolume";
    public const string BGM_VOLUME = "BGMVolume";
    public const string SFX_VOLUME = "SFXVolume";

    //[SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    //[SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;
    // Start is called before the first frame update

    float savedMasterVolume = 0.0f;
    float savedBGMVolume = 0.0f;
    float savedSFXVolume = 0.0f;

    void Start()
    {
        UpdateVolumes();
    }

    public void UpdateVolumes() {
        // get the new values from playerprefs...
        savedMasterVolume = PlayerPrefs.GetFloat(OptionsManager.MASTER_VALUE, 1f);
        savedBGMVolume = PlayerPrefs.GetFloat(OptionsManager.BGM_VALUE, 1f);
        savedSFXVolume = PlayerPrefs.GetFloat(OptionsManager.SFX_VALUE, 1f);

        // Set our mixer volumes to the correct volume.
        mixer.SetFloat(MASTER_VOLUME, savedMasterVolume > 0f ? Mathf.Log10(savedMasterVolume) * multiplier : Mathf.Log10(0.0000001f) * multiplier);
        mixer.SetFloat(BGM_VOLUME, savedBGMVolume > 0f ? Mathf.Log10(savedBGMVolume) * multiplier : Mathf.Log10(0.0000001f) * multiplier);
        mixer.SetFloat(SFX_VOLUME, savedSFXVolume > 0f ? Mathf.Log10(savedSFXVolume) * multiplier : Mathf.Log10(0.0000001f) * multiplier);
    }

    public void PreviewVolume(float master, float bgm, float sfx) {
        // Do not use the playerprefs here as we just want to preview this when the value is changed in the option screen here
        // Apply in options screen will adjust with UpdateVolumes() at the appropriate time
        mixer.SetFloat(MASTER_VOLUME, master > 0f ? Mathf.Log10(master) * multiplier : Mathf.Log10(0.0000001f) * multiplier);
        mixer.SetFloat(BGM_VOLUME, bgm > 0f ? Mathf.Log10(bgm) * multiplier : Mathf.Log10(0.0000001f) * multiplier);
        mixer.SetFloat(SFX_VOLUME, sfx > 0f ? Mathf.Log10(sfx) * multiplier : Mathf.Log10(0.0000001f) * multiplier);
    }
}
