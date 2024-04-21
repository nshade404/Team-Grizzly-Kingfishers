using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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

        playerInputActions.Player.ScrollSelectTurret.started += ScrollSelectTurret;
        playerInputActions.Player.SelectTurret1.started += SelectTurret1;
        playerInputActions.Player.SelectTurret2.started += SelectTurret2;
        playerInputActions.Player.SelectTurret3.started += SelectTurret3;
        playerInputActions.Player.SelectTurret4.started += SelectTurret4;
        playerInputActions.Player.SelectTurret5.started += SelectTurret5;
        playerInputActions.Player.SelectTurret6.started += SelectTurret6;
        playerInputActions.Player.PlaceTurret.started += PlaceTurret;
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

    public void ScrollSelectTurret(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started: // cycle selected turret forward/backwards with scroll wheel... Need to extend this somehow to a button also (Q and E?)
                int value = (int)context.ReadValue<Vector2>().y;
                if (value > 0) {
                    gameManager.instance.playerScript.selectTurret(1);
                }
                if (value < 0) {
                    gameManager.instance.playerScript.selectTurret(-1);
                }
                break;
        }
    }

    // Note: This block is not ideal... but don't have time to figure out a cleaner way to figure this out for the time being...
    public void SelectTurret1(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                SetSelectTurret(0);
                break;
        }
    }

    public void SelectTurret2(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                SetSelectTurret(1);
                break;
        }
    }

    public void SelectTurret3(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                SetSelectTurret(2);
                break;
        }
    }

    public void SelectTurret4(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                SetSelectTurret(3);
                break;
        }
    }

    public void SelectTurret5(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                SetSelectTurret(4);
                break;
        }
    }

    public void SelectTurret6(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                SetSelectTurret(5);
                break;
        }
    }
    
    public void SetSelectTurret(int index) {
        gameManager.instance.playerScript.SetSelectedTurret(index);
    }

    public void PlaceTurret(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                gameManager.instance.playerScript.PlaceTurret();
                break;
        }
    }

}
