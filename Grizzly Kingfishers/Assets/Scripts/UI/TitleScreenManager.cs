using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] GameObject TitleScreen;
    [SerializeField] GameObject OptionScreen;
    [SerializeField] GameObject CreditScreen;
    [SerializeField] GameObject LoadingScreen;

    [SerializeField] int secondsForLoadingScreen;

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
        TitleScreen.SetActive(true);
    }

    public void OnOptionsClicked() {
        HideAllScreens();
        OptionScreen.SetActive(true);
    }

    public void OnCreditsClicked() {
        HideAllScreens();
        CreditScreen.SetActive(true);
    }

    private void HideAllScreens() {
        TitleScreen.SetActive(false);
        OptionScreen.SetActive(false);
        CreditScreen.SetActive(false);
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
