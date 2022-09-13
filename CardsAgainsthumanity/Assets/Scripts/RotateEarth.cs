using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    public int rotationSpeed = 5;
    public int mouseDragSpeed = 5;
    public int scrollSpeed = 5;
    public int minZoom = -30;
    public int maxZoom = 50;

    Transform earthTf;
    Vector3 resetRotation;
    Vector3 resetPosition;

    void Start()
    {
        earthTf = GetComponent<Transform>();
        resetRotation = new Vector3(earthTf.rotation.x, earthTf.rotation.y, earthTf.rotation.z);
        resetPosition = new Vector3(earthTf.position.x, earthTf.position.y, earthTf.position.z);
    }

    void Update()
    {
        Dragging();

        Scrolling();

        ResetEarth();
    }

    void Dragging()
    {
        if (Input.GetMouseButton(1))
        {
            float rotX = Input.GetAxis("Mouse X") * mouseDragSpeed * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * mouseDragSpeed * Time.deltaTime;

            earthTf.RotateAround((Vector3.Dot(earthTf.up, Vector3.down) > 0) ? -earthTf.up : earthTf.up, -rotX); // Rotate around X axis
            earthTf.RotateAround(Vector3.right, rotY); // Rotate around Y axis
        }
        else
        {
            earthTf.Rotate(0, rotationSpeed * Time.deltaTime, 0); //Rotate if player is not dragging
        }
    }

    void Scrolling()
    {
        //Scroll by turning mousewheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && earthTf.position.z > minZoom) // forward
        {
            earthTf.position -= new Vector3(0, 0, scrollSpeed);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f && earthTf.position.z < maxZoom) // backwards
        {
            earthTf.position += new Vector3(0, 0, scrollSpeed);
        }
    }

    private void ResetEarth()
    {
        //Reset rotation and position to initial values
        if (Input.GetKey(KeyCode.Space))
        {
            earthTf.rotation = Quaternion.Euler(resetRotation);
            earthTf.position = resetPosition;
        }
    }
}
