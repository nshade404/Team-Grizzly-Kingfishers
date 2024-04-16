using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private List<OptionBtn> optionBtns;
    private OptionBtn selectedBtn;
    public Sprite btnIdle;
    public Sprite btnHover;
    public Sprite btnSelected;

    private void Start() {
        //ResetAllButtons();
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
}
