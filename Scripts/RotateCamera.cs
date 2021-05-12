using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        // Orbit around on Ctrl + < or Ctrl + > 
        if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))) { 
            float horizontalInput = Input.GetAxis("Horizontal"); // Horizontal axis because left and right arrow keys
            print("Orbit Around \n");
            transform.Rotate(Vector3.up,  -rotationSpeed * Time.deltaTime * horizontalInput);  // Time.deltaTime is used to not rotate every single frame
        }
    }
}
