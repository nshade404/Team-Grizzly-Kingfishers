using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputFunctions : MonoBehaviour
{
    public PlayerInputActions playerInputActions;

    private void Awake() {

        if(playerInputActions == null) {
            playerInputActions = new PlayerInputActions();
            LoadSavedBindings();
            UnbindAllActions();
        }
        playerInputActions.Player.Enable();
        // Shoot
        //playerInputActions.Player.Shoot.started += Shoot;
        // Jump
        playerInputActions.Player.Jump.started += Jump;
        playerInputActions.Player.Jump.canceled += Jump;
        // Sprint
        playerInputActions.Player.Sprint.started += Sprint;
        playerInputActions.Player.Sprint.canceled += Sprint;
        // Turret
        playerInputActions.Player.ScrollSelectTurret.started += ScrollSelectTurret;
        playerInputActions.Player.StepSelectTurretPos.started += StepSelectTurretPos;
        playerInputActions.Player.StepSelectTurretNeg.started += StepSelectTurretNeg;
        playerInputActions.Player.SelectTurret1.started += SelectTurret1;
        playerInputActions.Player.SelectTurret2.started += SelectTurret2;
        playerInputActions.Player.SelectTurret3.started += SelectTurret3;
        playerInputActions.Player.SelectTurret4.started += SelectTurret4;
        playerInputActions.Player.SelectTurret5.started += SelectTurret5;
        playerInputActions.Player.SelectTurret6.started += SelectTurret6;
        playerInputActions.Player.PlaceTurret.started += PlaceTurret;
    }

    public void UnbindAllActions() {
        // Shoot
        playerInputActions.Player.Shoot.started -= Shoot;
        // Jump
        playerInputActions.Player.Jump.started -= Jump;
        playerInputActions.Player.Jump.canceled -= Jump;
        // Sprint
        playerInputActions.Player.Sprint.started -= Sprint;
        playerInputActions.Player.Sprint.canceled -= Sprint;
        // Turret
        playerInputActions.Player.ScrollSelectTurret.started -= ScrollSelectTurret;
        playerInputActions.Player.StepSelectTurretPos.started -= StepSelectTurretPos;
        playerInputActions.Player.StepSelectTurretNeg.started -= StepSelectTurretNeg;
        playerInputActions.Player.SelectTurret1.started -= SelectTurret1;
        playerInputActions.Player.SelectTurret2.started -= SelectTurret2;
        playerInputActions.Player.SelectTurret3.started -= SelectTurret3;
        playerInputActions.Player.SelectTurret4.started -= SelectTurret4;
        playerInputActions.Player.SelectTurret5.started -= SelectTurret5;
        playerInputActions.Player.SelectTurret6.started -= SelectTurret6;
        playerInputActions.Player.PlaceTurret.started -= PlaceTurret;
    }

    public void LoadSavedBindings() {
        string rebounds = PlayerPrefs.GetString(OptionsManager.PLAYER_SAVED_REBOUND_KEYBINDS, PlayerPrefs.GetString(OptionsManager.PLAYER_DEFAULT_KEYBINDS));
        playerInputActions.LoadBindingOverridesFromJson(rebounds);
    }

    private void Update() {
        Move(playerInputActions.Player.Movement.ReadValue<Vector2>());
        Look(playerInputActions.Player.Camera.ReadValue<Vector2>());
    }

    private void Move(Vector2 inVector) {
        gameManager.instance.playerScript.MoveDir = new Vector3(inVector.x, 0, inVector.y); // pass this to player controller
    }

    private void Look(Vector2 inVector) {
        gameManager.instance.camController.MouseDir = inVector.normalized;
    }

    public void Shoot(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                gameManager.instance.playerScript.IsShooting = true;
                Debug.Log("Shoot Started");
                break;
            case InputActionPhase.Canceled:
                //gameManager.instance.playerScript.IsShooting = false;
                Debug.Log("Shoot Canceled");
                break;
        }
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
                    CycleSelectTurret(1);
                }
                if (value < 0) {
                    CycleSelectTurret(-1);
                }
                break;
        }
    }

    #region Keyboard Select Turrets
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
    #endregion

    public void PlaceTurret(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                gameManager.instance.playerScript.PlaceTurret();
                break;
        }
    }

    public void StepSelectTurretPos(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                CycleSelectTurret(1);
                break;
        }
    }

    public void StepSelectTurretNeg(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                CycleSelectTurret(-1);
                break;
        }
    }

    public void SetSelectTurret(int index) {
        gameManager.instance.playerScript.SetSelectedTurret(index);
    }

    private void CycleSelectTurret(int value) {
        gameManager.instance.playerScript.selectTurret(value);
    }

}
