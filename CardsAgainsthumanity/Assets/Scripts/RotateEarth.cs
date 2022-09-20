using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    [SerializeField]
    public int rotationSpeed = 5;
    [SerializeField]
    public int mouseDragSpeed = 5;
    [SerializeField]
    public int scrollSpeed = 5;
    [SerializeField]
    public int minZoom = -40;
    [SerializeField]
    public int maxZoom = 50;

    private bool isDragging = false;
    private Hand handScript;
    Transform earthTf;
    Vector3 resetRotation;
    Vector3 resetPosition;

    void Start()
    {
        handScript = GameObject.FindWithTag("Hand").GetComponent<Hand>();
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
        if (Input.GetMouseButton(1) && !handScript.CheckHoveringOfHandCards())
        {
            isDragging = true;
            float rotX = Input.GetAxis("Mouse X") * mouseDragSpeed * Time.unscaledDeltaTime;
            float rotY = Input.GetAxis("Mouse Y") * mouseDragSpeed * Time.unscaledDeltaTime;

            earthTf.RotateAround((Vector3.Dot(earthTf.up, Vector3.down) > 0) ? -earthTf.up : earthTf.up, -rotX); // Rotate around X axis
            earthTf.RotateAround(Vector3.right, rotY); // Rotate around Y axis
        }
        else
        {
            isDragging = false;
            earthTf.Rotate(0, rotationSpeed * Time.unscaledDeltaTime, 0); //Rotate if player is not dragging
        }
    }

    public bool IsDragging()
    {
        return isDragging;
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
