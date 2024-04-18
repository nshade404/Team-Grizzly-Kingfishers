using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Jump(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                gameManager.instance.playerScript.IsJumping = true;
                break;
            case InputActionPhase.Canceled:
                gameManager.instance.playerScript.IsJumping = false;
                break;
        }
    }

    public void Sprint(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                gameManager.instance.playerScript.IsSprinting = true;
                gameManager.instance.playerScript.Speed *= gameManager.instance.playerScript.GetSprintMod;
                break;
            case InputActionPhase.Canceled:
                gameManager.instance.playerScript.IsSprinting = false;
                gameManager.instance.playerScript.Speed /= gameManager.instance.playerScript.GetSprintMod;
                break;
        }
    }
}
