using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToEarthRotation : MonoBehaviour
{
    float earthRadius = 28;
    float objectHeigt = 0;

    void Start()
    {
        switch (tag)
        {
            case "Turbine":
                objectHeigt = 4;
                break;
            case "Nuclear":
                objectHeigt = 4.5f;
                break;
            case "Rocket":
                objectHeigt = 4;
                break;
            case "WaterPlant":
                objectHeigt = 4.8f;
                break;
            case "Crane":
                objectHeigt = 5.2f;
                break;
            case "Tree1":
                objectHeigt = 4.5f;
                break;
            case "House1":
                objectHeigt = 4.6f;
                break;
            case "Tree2":
                objectHeigt = 5f;
                break;
            case "City":
                objectHeigt = 4;
                break;
        }

        Transform objectTransform = GetComponent<Transform>();
        Transform earthTf = GameObject.FindGameObjectWithTag("Earth").transform;
        Vector3 delta = objectTransform.position - earthTf.position;

        objectTransform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
        objectTransform.Rotate(90, 0, 0);

        float dist = delta.magnitude - earthRadius - objectHeigt / 2;
        objectTransform.position -= objectTransform.up * dist;
    }
}
