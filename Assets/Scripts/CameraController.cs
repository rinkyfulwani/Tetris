using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // focusPoint is the point the camera is looking at
    public Transform focusPoint;
    // vector for holding previous mouse position
    private Vector3 previousMousePosition;
    // config for the mouse sensitivity
    private float mouseSens = 0.3f;

    public static CameraController currentInstance;

    private void Awake() {
        currentInstance = this;
    }


    // Update is called once per frame
    private void Update()
    {
        // make sure that the camera is focusing on the focusPoint
        transform.LookAt(focusPoint);

        // call the logic for the mouse button clicks
        MouseButtonHandler();

    }

    public void MoveFocusPoint(Vector3 position) {
        focusPoint.transform.position = position;
    }

    // logic for the mouse button clicks
    private void MouseButtonHandler() {
        // save the previous position when clicking the left mouse button
        if (Input.GetMouseButtonDown(0)) {
            SaveCurrentMousePosition();
        }
        // call the Rotate function each frame if left button is pressed or held 
        if(Input.GetMouseButton(0)) {
            Rotate();
        }
    }

    // save the current mouse position in a variable
    private void SaveCurrentMousePosition() {
        previousMousePosition = Input.mousePosition;
    }

    // rotate the camera around the X and Y axis
    private void Rotate()  {
        Vector3 diff = Input.mousePosition - previousMousePosition;
        // save the new angles from the difference of positions
        float yAngleDiff = -diff.y * mouseSens;
        float xAngleDiff = diff.x * mouseSens;

        // rotate the focusPoint around the x axis
        Vector3 focusPointAngles = focusPoint.transform.eulerAngles;
        focusPointAngles.x += yAngleDiff;
        focusPoint.transform.eulerAngles = focusPointAngles;

        // rotate the focusPoint around the y axis with the diff in xAngle
        // inputs are 1. which position 2. which vector direction 3. with what angle.
        focusPoint.RotateAround(focusPoint.position, Vector3.up, xAngleDiff);
        SaveCurrentMousePosition();
    }

}


