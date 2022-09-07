using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEarth : MonoBehaviour
{
    public int rotationSpeed = 50;

    Transform earthTf;
    Vector3 resetRotation;      

    void Start()
    {
        earthTf = GetComponent<Transform>();
        resetRotation = new Vector3(earthTf.rotation.x, earthTf.rotation.y, earthTf.rotation.z);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            earthTf.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            earthTf.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }

        if (Input.GetMouseButton(0))
        {
            float rotX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            //earthTf.Rotate(earthTf.up, -rotX);
            //earthTf.Rotate(Vector3.right, rotY);

            // Rotate around X axis
            earthTf.RotateAround((Vector3.Dot(transform.up, Vector3.down) > 0) ? -earthTf.up : earthTf.up, -rotX);

            // Rotate around Y axis
            earthTf.RotateAround(Vector3.right, rotY);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            earthTf.rotation = Quaternion.Euler(resetRotation);
        }
    }
}
