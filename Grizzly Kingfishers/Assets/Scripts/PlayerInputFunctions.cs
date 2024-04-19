using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputFunctions : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    private void Awake() {

        if(playerInputActions == null) {
            playerInputActions = new PlayerInputActions();
            LoadSavedBindings();
        }
        playerInputActions.Player.Enable();
        // Jump
        playerInputActions.Player.Jump.started += Jump;
        playerInputActions.Player.Jump.canceled += Jump;
        // Sprint
        playerInputActions.Player.Sprint.started += Sprint;
        playerInputActions.Player.Sprint.canceled += Sprint;
    }

    public void LoadSavedBindings() {
        string rebounds = PlayerPrefs.GetString(OptionsManager.PLAYER_SAVED_REBOUND_KEYBINDS, PlayerPrefs.GetString(OptionsManager.PLAYER_DEFAULT_KEYBINDS));
        playerInputActions.LoadBindingOverridesFromJson(rebounds);
    }

    private void Update() {
        Move(playerInputActions.Player.Movement.ReadValue<Vector2>());
    }

    private void Move(Vector2 inVector) {
        gameManager.instance.playerScript.MoveDir = new Vector3(inVector.x, 0, inVector.y); // pass this to player controller
    }

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
