using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    // Keys for saving and retrieving from PlayerPrefs
    public const int VOLUME_MAX = 100;
    public const string MASTER_VALUE = "MasterValue";
    public const string BGM_VALUE = "BGMValue";
    public const string SFX_VALUE = "MasterValue";

    public const string PLAYER_DEFAULT_KEYBINDS = "PlayerDefaultKeybinds";
    public const string PLAYER_SAVED_REBOUND_KEYBINDS = "PlayerSavedReboundKeybinds";


    [SerializeField] KeybindItem[] keybinds;
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
    PlayerInputActions pia;

    string reboundKeybinds;

    private void Start() {
        applyButton.interactable = false;

        masterSlider.maxValue = VOLUME_MAX;
        bgmSlider.maxValue = VOLUME_MAX;
        sfxSlider.maxValue = VOLUME_MAX;

        // Load in saved values.
        masterSlider.value = (int)(PlayerPrefs.GetFloat(MASTER_VALUE, 1) * VOLUME_MAX);
        bgmSlider.value = (int)(PlayerPrefs.GetFloat(BGM_VALUE, 1) * VOLUME_MAX);
        sfxSlider.value = (int)(PlayerPrefs.GetFloat(SFX_VALUE, 1) * VOLUME_MAX);

        pia = new PlayerInputActions();
        PlayerPrefs.SetString(PLAYER_DEFAULT_KEYBINDS, pia.SaveBindingOverridesAsJson());
        // Check if we have any saved rebindigns.... if not, load the defaults.
        string rebounds = PlayerPrefs.GetString(PLAYER_SAVED_REBOUND_KEYBINDS, PlayerPrefs.GetString(PLAYER_DEFAULT_KEYBINDS));
        pia.LoadBindingOverridesFromJson(rebounds);

        DisplayAllKeybinds();
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

        PlayerPrefs.SetString(PLAYER_SAVED_REBOUND_KEYBINDS, pia.SaveBindingOverridesAsJson());

        applyButton.interactable = false;
        optionPendingChange = false;
    }

    public void BackButtonClicked() {
        // Check if any pending options need to be saved...
        if (optionPendingChange) {
            // if so, show pop up asking if they want to save or discard changes
            Debug.Log("Attempted to back out with pending changes!");
            
        } else {
            CloseOptionsScreen();
        }
    }

    private void CloseOptionsScreen() {
        if (titleScreenManager != null) { // if we are on title screen, this will be set, use this funcitonality...
            titleScreenManager.ReturnToTitle();
        }
        else { // otherwise we are in game, return to game...
            gameObject.SetActive(false);
        }
    }

    #region keyrebinding

    private void DisplayAllKeybinds() {

        for (int i = 0; i < keybinds.Length; i++) {
            string boundKey = "";

            if (keybinds[i].bindingName == "Up") { // Movement forward
                boundKey = pia.FindAction("Movement")?.GetBindingDisplayString(1);
            }
            else if(keybinds[i].bindingName == "Down") { // Movement backwards
                boundKey = pia.FindAction("Movement")?.GetBindingDisplayString(2);
            }
            else if (keybinds[i].bindingName == "Left") { // Movement left
                boundKey = pia.FindAction("Movement")?.GetBindingDisplayString(3);
            }
            else if (keybinds[i].bindingName == "Right") { // Movement right
                boundKey = pia.FindAction("Movement")?.GetBindingDisplayString(4);
            }
            else { // everything else which we currently only expec one bind on...
                boundKey = pia.FindAction(keybinds[i].bindingName)?.GetBindingDisplayString(0);
            }

            keybinds[i].boundText.text = boundKey;
            Debug.Log(keybinds[i].bindingName + " has " + boundKey);
        }
    }

    public void RebindKey(KeybindItem keybindItem) {

        // Verify this is actually disabled.
        pia.Player.Disable();

        string itemName = keybindItem.bindingName;
        switch (itemName) {
            case "Jump":
                pia.Player.Jump.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnComplete(callback => {
                        Debug.Log(callback);
                        keybindItem.boundText.text = pia.Player.Jump.GetBindingDisplayString(0);
                        callback.Dispose();
                        
                    }).Start();
                break;
            case "Up":
                pia.Player.Movement.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnComplete(callback => {
                        Debug.Log(callback);
                        keybindItem.boundText.text = pia.Player.Movement.GetBindingDisplayString(1);
                        Debug.Log("Up " + pia.Player.Movement.GetBindingDisplayString(1));
                        callback.Dispose();
                    }).Start();
                break;
            case "Down":
                pia.Player.Movement.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnComplete(callback => {
                        Debug.Log(callback);
                        keybindItem.boundText.text = pia.Player.Movement.GetBindingDisplayString(2);
                        Debug.Log("Down " + pia.Player.Movement.GetBindingDisplayString(2));
                        callback.Dispose();
                    }).Start();
                break;
            case "Left":
                pia.Player.Movement.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnComplete(callback => {
                        Debug.Log(callback);
                        keybindItem.boundText.text = pia.Player.Movement.GetBindingDisplayString(3);
                        Debug.Log("Left " + pia.Player.Movement.GetBindingDisplayString(3));
                        callback.Dispose();
                    }).Start();
                break;
            case "Right":
                pia.Player.Movement.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnComplete(callback => {
                        Debug.Log(callback);
                        keybindItem.boundText.text = pia.Player.Movement.GetBindingDisplayString(4);
                        Debug.Log("Right " + pia.Player.Movement.GetBindingDisplayString(4));
                        callback.Dispose();
                    }).Start();
                break;
            case "Sprint":
                pia.Player.Sprint.PerformInteractiveRebinding()
                    .WithControlsExcluding("Mouse")
                    .OnComplete(callback => {
                        Debug.Log(callback);
                        keybindItem.boundText.text = pia.Player.Sprint.GetBindingDisplayString(0);
                        callback.Dispose();

                    }).Start();
                break;
        }

        applyButton.interactable = true;
    }

    #endregion
}
