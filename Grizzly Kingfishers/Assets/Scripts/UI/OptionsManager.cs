using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    // Keys for saving and retrieving from PlayerPrefs
    public const int VOLUME_MAX = 100;
    public const string MASTER_VALUE = "MasterValue";
    public const string BGM_VALUE = "BGMValue";
    public const string SFX_VALUE = "MasterValue";



    [SerializeField] private List<OptionBtn> optionBtns;
    [SerializeField] TitleScreenManager titleScreenManager;
    [SerializeField] Button applyButton;
    private OptionBtn selectedBtn;
    public Sprite btnIdle;
    public Sprite btnHover;
    public Sprite btnSelected;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    bool optionPendingChange;

    private void Start() {
        applyButton.interactable = false;

        masterSlider.maxValue = VOLUME_MAX;
        bgmSlider.maxValue = VOLUME_MAX;
        sfxSlider.maxValue = VOLUME_MAX;

        // Load in saved values.
        masterSlider.value = (int)(PlayerPrefs.GetFloat(MASTER_VALUE, 1) * VOLUME_MAX);
        bgmSlider.value = (int)(PlayerPrefs.GetFloat(BGM_VALUE, 1) * VOLUME_MAX);
        sfxSlider.value = (int)(PlayerPrefs.GetFloat(SFX_VALUE, 1) * VOLUME_MAX);
    }

    /// <summary>
    /// What happens when we click on an option screen button
    /// </summary>
    public void OnButtonSelect(OptionBtn btn) {
        selectedBtn = btn;
        ResetAllButtons();
        btn.btnBackground.sprite = btnSelected;
        btn.optionScreen.SetActive(true);
    }

    /// <summary>
    /// What we do when we mouse over an option screen button
    /// </summary>
    public void OnButtonEnter(OptionBtn btn) {
        if(btn == selectedBtn) {
            return;
        }

        ResetAllButtons();
        btn.btnBackground.sprite = btnHover;
    }

    /// <summary>
    /// What we do when we exit an option screen button.
    /// </summary>
    public void OnButtonExit(OptionBtn btn) {
        ResetAllButtons();
    }

    /// <summary>
    /// Resets all buttons except the selected button back to default states.
    /// </summary>
    private void ResetAllButtons() {
        foreach(OptionBtn btn in optionBtns) {
            // If we have a currently selected button, don't change it...
            if(selectedBtn != null && btn == selectedBtn) {
                continue;
            }
            btn.btnBackground.sprite = btnIdle;
            btn.optionScreen.SetActive(false);
        }
    }

    public void SliderValueChanged(int index) {
        switch (index) {
            case 0: // Master slider
                if ((int)(PlayerPrefs.GetFloat(MASTER_VALUE, 1) * VOLUME_MAX) != masterSlider.value) {
                    // We have a new value, prompt an update to be saved
                    optionPendingChange = true;
                }
                break;
            case 1: // BGM slider
                if ((int)(PlayerPrefs.GetFloat(BGM_VALUE, 1) * VOLUME_MAX) != bgmSlider.value) {
                    // We have a new value, prompt an update to be saved
                    optionPendingChange = true;
                }
                break;
            case 2: // SFx Slider
                if ((int)(PlayerPrefs.GetFloat(SFX_VALUE, 1) * VOLUME_MAX) != sfxSlider.value) {
                    // We have a new value, prompt an update to be saved
                    optionPendingChange = true;
                }
                break;
        }

        if (optionPendingChange) {
            applyButton.interactable = true;
        }
    }

    public void ApplyChanges() {
        // apply all pending changes to playerprefs.
        PlayerPrefs.SetFloat(MASTER_VALUE, masterSlider.normalizedValue);
        PlayerPrefs.SetFloat(BGM_VALUE, bgmSlider.normalizedValue);
        PlayerPrefs.SetFloat(SFX_VALUE, sfxSlider.normalizedValue);

        applyButton.interactable = false;
        optionPendingChange = false;
    }

    public void BackButtonClicked() {
        // Check if any pending options need to be saved...
        if (optionPendingChange) {
            // if so, show pop up asking if they want to save or discard changes
            
        } else {
            CloseOptionsScreen();
        }
    }

    private void CloseOptionsScreen() {
        if (titleScreenManager != null) { // if we are on title screen, this will be set, use this funcitonality...
            titleScreenManager.ReturnToTitle();
        }
        else { // otherwise we are in game, return to game...

        }
    }
}
