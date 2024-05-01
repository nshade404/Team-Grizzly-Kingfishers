using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputFunctions : MonoBehaviour
{
    public UIInputActions uia;

    private void Awake() {
        if(uia == null) {
            uia = new UIInputActions();
            UnbindAllActions();
        }

        uia.UI.Enable();

        uia.UI.Cancel.performed += CancelPressed;
        //uia.UI.Pause.performed += StartPressed;
    }

    public void UnbindAllActions() {
        uia.UI.Cancel.performed -= CancelPressed;
        //uia.UI.Pause.performed -= StartPressed;
    }

    private void StartPressed(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Performed:
                if (gameManager.instance != null) {
                    //gameManager.instance.StartPressed();
                }
                break;
        }
    }

    private void CancelPressed(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Performed:
                if(gameManager.instance != null) {
                    gameManager.instance.CancelPressed();
                }
                break;
        }
    }
}
