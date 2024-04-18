using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputFunctions : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    private void Awake() {

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        // Jump
        playerInputActions.Player.Jump.started += Jump;
        playerInputActions.Player.Jump.canceled += Jump;
        // Sprint
        playerInputActions.Player.Sprint.started += Sprint;
        playerInputActions.Player.Sprint.canceled += Sprint;
        // Movement
        //playerInputActions.Player.Movement.performed += Move;
    }

    private void Update() {
        Move(playerInputActions.Player.Movement.ReadValue<Vector2>());
    }

    private void Move(Vector2 inVector) {
        //moveDir = new(Vector3(inVector.x, 0, inVector.y); // pass this to player controller
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
