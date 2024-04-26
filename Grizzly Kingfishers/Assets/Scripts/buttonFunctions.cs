using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.player.GetComponent<PlayerInputFunctions>()?.UnbindAllActions();
        gameManager.instance.stateUnpaused();
    }

    public void quit()
    {
        gameManager.instance.stateUnpaused();
        gameManager.instance.player.GetComponent<PlayerInputFunctions>()?.UnbindAllActions();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gameManager.instance.loadingScreen.SetActive(true);
        SceneManager.LoadScene(0);
        //#if UNITY_EDITOR
        //UnityEditor.EditorApplication.isPlaying = false;
        //#else
        //Application.Quit();
        //#endif
    }

    public void showOptions() {
        gameManager.instance.optionScreen.GetComponent<OptionsManager>().OpenOptions();
        gameManager.instance.optionScreen.SetActive(true);
        gameManager.instance.menuActive = gameManager.instance.optionScreen;
    }
    
}
