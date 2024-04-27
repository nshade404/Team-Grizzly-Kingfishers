using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject OptionScreen;
    [SerializeField] GameObject CreditScreen;
    [SerializeField] GameObject LoadingScreen;

    // First buttons selectable.
    [SerializeField] GameObject titleFirst;
    [SerializeField] GameObject optionFirst;
    [SerializeField] GameObject creditFirst;

    [SerializeField] int secondsForLoadingScreen;

    [SerializeField] VolumeControl volumeControl;

    public VolumeControl GetVolumeControl() { return volumeControl; }

    private void Start() {
        ReturnToTitle();
    }

    /// <summary>
    /// close this level and load the game up.
    /// </summary>
    public void OnPlayClicked() {
        LoadingScreen.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //IEnumerator RunLoadScreen() {

    //}

    public void ReturnToTitle(bool fromOptions = false) {
        HideAllScreens();
        EventSystem.current.SetSelectedGameObject(titleFirst);
        TitleScreen.SetActive(true);
    }

    public void OnOptionsClicked() {
        HideAllScreens();
        EventSystem.current.SetSelectedGameObject(optionFirst);
        OptionScreen.GetComponent<OptionsManager>().OpenOptions();
        OptionScreen.SetActive(true);
    }

    public void OnCreditsClicked() {
        HideAllScreens();
        EventSystem.current.SetSelectedGameObject(creditFirst);
        CreditScreen.SetActive(true);
    }

    private void HideAllScreens() {
        TitleScreen.SetActive(false);
        OptionScreen.SetActive(false);
        CreditScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    public void OnQuitClicked() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
