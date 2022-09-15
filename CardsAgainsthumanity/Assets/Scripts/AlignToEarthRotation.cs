using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToEarthRotation : MonoBehaviour
{
    float earthRadius = 16;
    float objectHeigt = 0;

    void Start()
    {
        switch (tag)
        {
            case "Turbine":
                objectHeigt = 4;
                break;
            case "Nuclear":
                objectHeigt = 4;
                break;
            case "Rocket":
                objectHeigt = 1;
                break;
            case "WaterPlant":
                objectHeigt = 5;
                break;
            case "Crane":
                objectHeigt = 5;
                break;
            case "Tree1":
                objectHeigt = 5;
                break;
            case "House1":
                objectHeigt = 5;
                break;
        }

        Transform objectTransform = GetComponent<Transform>();
        Transform earthTf = GameObject.FindGameObjectWithTag("Earth").transform;
        Vector3 delta = objectTransform.position - earthTf.position;

        objectTransform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
        objectTransform.Rotate(90, 0, 0);

        float dist = delta.magnitude - (earthRadius + objectHeigt / 2);
        objectTransform.position -= objectTransform.up * dist;
    }
}
