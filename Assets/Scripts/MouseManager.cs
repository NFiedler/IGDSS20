using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseManager : MonoBehaviour
{
    private UnityEngine.Vector3 prevMousePosition;
    private Boolean previousClickWasRight;
    public int xAndZScrollFactor = 3;
    public int Scrollfactor = 3;
    public int maxHeight = 50;
    public int minHeight = 20;

    public 
    // Start is called before the first frame update
    void Start()
    {
        prevMousePosition = Input.mousePosition;
        // this is just a random position that is over the tiles
        UnityEngine.Vector3 cameraStartPosition = new UnityEngine.Vector3(80, 50, 40);
        Camera.main.transform.position = cameraStartPosition;
        previousClickWasRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            UnityEngine.Vector3 yChange = new UnityEngine.Vector3(0, Input.mouseScrollDelta.y, 0);
            Camera.main.transform.position += -yChange * Scrollfactor;

            // limit max height
            if(Camera.main.transform.position.y > maxHeight){
                Camera.main.transform.position = new UnityEngine.Vector3(Camera.main.transform.position.x, maxHeight, Camera.main.transform.position.z);
            }
            // limit min height
            if (Camera.main.transform.position.y < minHeight)
            {
                Camera.main.transform.position = new UnityEngine.Vector3(Camera.main.transform.position.x, minHeight, Camera.main.transform.position.z);
            }
        }

        // we need to constantly reevaluate the last mouse position
        UnityEngine.Vector3 currentMousePosition = Input.mousePosition;
        if (Input.GetMouseButton(1)) // right mouse click
        {
            // use the movement measurement only if the measurement happened while we had right clicked previously
            if (previousClickWasRight)
            {
                UnityEngine.Vector3 diffMousePosition = currentMousePosition - prevMousePosition;
                // we only want the change to happen in x and z position, so we put y to zero
                // y would be scrolling in/out which we will handle via mousewheel
                UnityEngine.Vector3 noYDiffMousePosition = new UnityEngine.Vector3(diffMousePosition.x, 0, diffMousePosition.y);
                // we invert it since it feels more natural for it to be drag and drop if it's done while the mouse button is pressed
                UnityEngine.Vector3 invertedDiff = -noYDiffMousePosition / xAndZScrollFactor;


                // move the camera by just as many pixels as the mouse moved since the last update
                Camera.main.transform.position += invertedDiff;
            }
            previousClickWasRight = true;

        }
        else
        {
            previousClickWasRight = false;
        }
        // after we used the position, we save the last position. This allows us to compare it to the next one to see the delta.
        prevMousePosition = currentMousePosition;


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button was pressed")
        }

    }
}
