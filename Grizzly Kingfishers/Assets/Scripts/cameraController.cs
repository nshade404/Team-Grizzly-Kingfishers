using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] float sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    public Vector3 MouseDir { get; set; } = Vector3.zero;

    [SerializeField] private float adjustedSensitivity = 0f;

    float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.forward = transform.parent.forward;
        UpdateSensitivityFromPrefs();
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        //float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        //float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = MouseDir.y * Time.deltaTime * (sensitivity + adjustedSensitivity);
        float mouseX = MouseDir.x * Time.deltaTime * (sensitivity + adjustedSensitivity);
      
        // invert look up/down
        if (invertY) {
            rotX += mouseY;
        }
        else {
            rotX -= mouseY;
        }

        // clamp rot on the x-axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // rotate the cam on the x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // rotate the player on the y-axis
        transform.parent.Rotate(mouseX * Vector3.up);
    }

    public void UpdateSensitivityFromPrefs() {
        float modifier = PlayerPrefs.GetFloat(OptionsManager.LOOK_SENSITIVITY, 0f);
        adjustedSensitivity = (sensitivity / 2) * modifier;
    }
}
