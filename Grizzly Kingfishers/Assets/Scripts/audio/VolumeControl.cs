using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 5f;
    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
    }
    private void Awake()
    {
        slider.onValueChanged.AddListener(handleSlider);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, slider.value);
    }

    private void handleSlider(float value)
    {
        mixer.SetFloat(volumeParameter, MathF.Log10(value) * multiplier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
