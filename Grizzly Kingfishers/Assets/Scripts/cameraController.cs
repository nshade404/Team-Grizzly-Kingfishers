using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        transform.forward = transform.parent.forward;
    }

    // Update is called once per frame
    void Update()
    {
        // get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

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
}
