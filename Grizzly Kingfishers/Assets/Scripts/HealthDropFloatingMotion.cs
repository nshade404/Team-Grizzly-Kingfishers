using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropFloatingMotion : MonoBehaviour
{
    public float floatHeight = 0.5F; // Height the object will float above its initial position
    public float floatSpeed = 1.0F; // Speed the object will float

    private Vector3 objectStartPos;

    void Start()
    {
        objectStartPos = transform.position;
    }

    void Update()
    {
        float newY = objectStartPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
