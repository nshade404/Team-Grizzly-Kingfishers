using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertmin, lockVertmax;
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
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensitivity;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }
        //lock vertical rotations
        rotX = Mathf.Clamp(rotX, lockVertmin, lockVertmax);
        //rotate camera on x-axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        //rotate player on y axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
