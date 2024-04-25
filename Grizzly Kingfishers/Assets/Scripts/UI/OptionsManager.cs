using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    // Keys for saving and retrieving from PlayerPrefs
    public const int VOLUME_MAX = 100;
    public const string MASTER_VALUE = "MasterValue";
    public const string BGM_VALUE = "BGMValue";
    public const string SFX_VALUE = "SFXValue";

    public const string LOOK_SENSITIVITY = "LookSensitivity";

    public const string PLAYER_DEFAULT_KEYBINDS = "PlayerDefaultKeybinds";
    public const string PLAYER_SAVED_REBOUND_KEYBINDS = "PlayerSavedReboundKeybinds";


    [SerializeField] KeybindItem[] keybinds;
    [SerializeField] private List<OptionBtn> optionBtns;
    [SerializeField] TitleScreenManager titleScreenManager;
    [SerializeField] Button applyButton;
    [SerializeField] Button backButton;
    [SerializeField] GameObject BackClickedWindow;
    [SerializeField] GameObject IsKeybindingWindow;
    [SerializeField] GameObject ResetBindingsWindow;
    [SerializeField] private OptionBtn selectedBtn;
    //public Sprite btnIdle;
    //public Sprite btnHover;
    //public Sprite btnSelected;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider lookSlider;

    bool optionPendingChange;
    PlayerInputActions pia;

    string reboundKeybinds;

    [SerializeField] bool DeleteBindings = false;

    private void Start() {
        applyButton.interactable = false;

        masterSlider.maxValue = VOLUME_MAX;
        bgmSlider.maxValue = VOLUME_MAX;
        sfxSlider.maxValue = VOLUME_MAX;
        // Load in saved values.
        masterSlider.value = (int)(PlayerPrefs.GetFloat(MASTER_VALUE, 1) * VOLUME_MAX);
        bgmSlider.value = (int)(PlayerPrefs.GetFloat(BGM_VALUE, 1) * VOLUME_MAX);
        sfxSlider.value = (int)(PlayerPrefs.GetFloat(SFX_VALUE, 1) * VOLUME_MAX);
        lookSlider.value = (PlayerPrefs.GetFloat(LOOK_SENSITIVITY, 0)); // if we have an pref for it, use that, otherwise use 0.

        if (DeleteBindings) {
            PlayerPrefs.DeleteKey(PLAYER_DEFAULT_KEYBINDS);
            PlayerPrefs.DeleteKey(PLAYER_SAVED_REBOUND_KEYBINDS);

            PlayerPrefs.SetString(PLAYER_DEFAULT_KEYBINDS, pia.SaveBindingOverridesAsJson());
        }

        OnButtonSelect(optionBtns[0]);
    }

    /// <summary>
    /// Used to setup any information we require when we first open the Options menu.
    /// </summary>
    public void OpenOptions() {
        if (pia == null) {
            pia = new PlayerInputActions();
        }

        string reboundKeybinds = PlayerPrefs.GetString(PLAYER_SAVED_REBOUND_KEYBINDS, PlayerPrefs.GetString(PLAYER_DEFAULT_KEYBINDS));
        pia.LoadBindingOverridesFromJson(reboundKeybinds);

        DisplayAllKeybinds();
        optionPendingChange = false;
    }

    /// <summary>
    /// What happens when we click on an option screen button
    /// </summary>
    public void OnButtonSelect(OptionBtn btn) {
        selectedBtn = btn;
        ResetAllButtons();
        //btn.btnBackground.sprite = btnSelected;
        btn.optionScreen.SetActive(true);

        // Set the apply buttons navigation dynamically to be the currently selected button.
        Navigation newApplyNav = new Navigation();
        newApplyNav.mode = Navigation.Mode.Explicit;
        newApplyNav.selectOnUp = btn.GetComponent<Button>();
        newApplyNav.selectOnRight = backButton;
        applyButton.navigation = newApplyNav;

        Navigation newBackNav = new Navigation();
        newBackNav.mode = Navigation.Mode.Explicit;
        newBackNav.selectOnUp = btn.GetComponent<Button>();
        newBackNav.selectOnLeft = applyButton;
        backButton.navigation = newBackNav;
    }

    /// <summary>
    /// What we do when we mouse over an option screen button
    /// </summary>
    public void OnButtonEnter(OptionBtn btn) {
        if(btn == selectedBtn) {
            return;
        }

        ResetAllButtons();
        //btn.btnBackground.sprite = btnHover;
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
        //foreach(OptionBtn btn in optionBtns) {
        //    // If we have a currently selected button, don't change it...
        //    if(selectedBtn != null && btn == selectedBtn) {
        //        continue;
        //    }
        //    //btn.btnBackground.sprite = btnIdle;
        //    btn.optionScreen.SetActive(false);
        //}
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
            case 3: // Look Slider
                if ((PlayerPrefs.GetFloat(LOOK_SENSITIVITY, 0)) != lookSlider.value) {
                    // We have a new value, prompt an update to be saved
                    Debug.Log(lookSlider.value + " Look Sensitivity");
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
        PlayerPrefs.SetFloat(LOOK_SENSITIVITY, lookSlider.value);

        PlayerPrefs.SetString(PLAYER_SAVED_REBOUND_KEYBINDS, pia.SaveBindingOverridesAsJson());
        //pia.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_SAVED_REBOUND_KEYBINDS));

        // Load any new bindings that are setup.
        if (gameManager.instance != null) {
            gameManager.instance.player.GetComponent<PlayerInputFunctions>().LoadSavedBindings();
            // Update the players camera sensitivity
            gameManager.instance.camController.UpdateSensitivityFromPrefs();
        }

        PlayerPrefs.Save();
        EventSystem.current.SetSelectedGameObject(backButton.gameObject);
        applyButton.interactable = false;
        optionPendingChange = false;
        BackClickedWindow.SetActive(false);
    }

    public void BackButtonClicked() {
        // Check if any pending options need to be saved...
        if (optionPendingChange) {
            // if so, show pop up asking if they want to save or discard changes
            BackClickedWindow.SetActive(true);
        } else {
            CloseOptionsScreen();
            pia.Player.Enable();
        }
    }

    public void BackButtonYesClicked() {
        ApplyChanges();
        pia.Player.Enable();
        CloseOptionsScreen();
    }

    public void BackButtonNoClicked() {
        BackClickedWindow.SetActive(false);
        CloseOptionsScreen();
    }

    private void CloseOptionsScreen() {
        if (titleScreenManager != null) { // if we are on title screen, this will be set, use this funcitonality...
            titleScreenManager.ReturnToTitle();

        }
        else { // otherwise we are in game, return to game...
            BackClickedWindow.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    #region keyrebinding

    private void DisplayAllKeybinds() {

        for (int i = 0; i < keybinds.Length; i++) {
            InputAction action = pia.FindAction(keybinds[i].bindingName);
            if (keybinds[i].boundKeyboardText != null) {
                string boundKey = action.GetBindingDisplayString(keybinds[i].keyboardBindIndex);
                keybinds[i].boundKeyboardText.text = boundKey;
            }
            if (keybinds[i].boundGamepadText != null) {
                string gamepadKey = action.GetBindingDisplayString(keybinds[i].gamepadBindIndex);
                keybinds[i].boundGamepadText.text = gamepadKey;
            }
        }
    }

    public void InteractiveKeyboardRebinding(KeybindItem keybindItem) {
        // Verify this is actually disabled.
        pia.Player.Disable();

        InputAction action = pia.FindAction(keybindItem.bindingName);
        if(action != null) {
            action.PerformInteractiveRebinding(keybindItem.keyboardBindIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .WithControlsExcluding("Mouse")
                .WithControlsExcluding("Gamepad")
                .OnCancel(
                    operation => {
                        IsKeybindingWindow.SetActive(false);
                    })
                .OnComplete(callback => {
                    keybindItem.boundKeyboardText.text = action.GetBindingDisplayString(keybindItem.keyboardBindIndex);
                    Debug.Log(keybindItem.bindingName + " " + keybindItem.keyboardBindIndex + " " + action.GetBindingDisplayString(keybindItem.keyboardBindIndex));
                    Debug.Log(action.bindings[keybindItem.keyboardBindIndex].overridePath);
                    //action.ApplyBindingOverride(keybindItem.bindingIndex, action.bindings[keybindItem.bindingIndex].overridePath);
                    IsKeybindingWindow.SetActive(false);
                    applyButton.interactable = true;
                    optionPendingChange = true;
                    callback.Dispose();
                }).Start();
            IsKeybindingWindow.SetActive(true);
        }
    }

    public void InteractiveGamepadRebinding(KeybindItem keybindItem) {
        // Verify this is actually disabled.
        pia.Player.Disable();

        InputAction action = pia.FindAction(keybindItem.bindingName);
        if (action != null) {
            action.PerformInteractiveRebinding(keybindItem.gamepadBindIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .WithControlsExcluding("Mouse")
                .WithControlsExcluding("Keyboard")
                .OnCancel(
                    operation => {
                        IsKeybindingWindow.SetActive(false);
                    })
                .OnComplete(callback => {
                    keybindItem.boundGamepadText.text = action.GetBindingDisplayString(keybindItem.gamepadBindIndex);
                    Debug.Log(keybindItem.bindingName + " " + keybindItem.gamepadBindIndex + " " + action.GetBindingDisplayString(keybindItem.gamepadBindIndex));
                    Debug.Log(action.bindings[keybindItem.gamepadBindIndex].overridePath);
                    //action.ApplyBindingOverride(keybindItem.bindingIndex, action.bindings[keybindItem.bindingIndex].overridePath);
                    IsKeybindingWindow.SetActive(false);
                    applyButton.interactable = true;
                    optionPendingChange = true;
                    callback.Dispose();
                }).Start();
            IsKeybindingWindow.SetActive(true);
        }
    }

    public void InitiateResetBindings() {
        ResetBindingsWindow.SetActive(true);
    }

    public void CancelResetBindings() {
        ResetBindingsWindow.SetActive(false);
    }

    public void ResetBindingsToDefault() {
        PlayerPrefs.DeleteKey(PLAYER_SAVED_REBOUND_KEYBINDS);
        gameManager.instance.player.GetComponent<PlayerInputFunctions>()?.LoadSavedBindings();
        ResetBindingsWindow.SetActive(false);
        
        DisplayAllKeybinds();
    }

    #endregion
}
