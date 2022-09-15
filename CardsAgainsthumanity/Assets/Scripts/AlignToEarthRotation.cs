using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToEarthRotation : MonoBehaviour
{
    float earthRadius = 44;
    float objectRadius = 0;

    void Start()
    {
        switch (tag)
        {
            case "Turbine":
                objectRadius = 5;
                break;
            case "Nuclear":
                objectRadius = 4;
                break;
            case "Rocket":
                objectRadius = 1;
                break;
            case "WaterPlant":
                objectRadius = 6;
                break;
            case "Crane":
                objectRadius = 5;
                break;
            case "Tree1":
                objectRadius = 6;
                break;
            case "House1":
                objectRadius = 6;
                break;
        }

        Transform objectTransform = GetComponent<Transform>();
        Transform earthTf = GameObject.FindGameObjectWithTag("Earth").transform;
        Vector3 delta = (objectTransform.position - earthTf.position).normalized;
        objectTransform.rotation = Quaternion.LookRotation(delta, Vector3.up);
        objectTransform.Rotate(90, 0, 0);

        float dist = (objectTransform.position - earthTf.position).magnitude - (earthRadius / 2 + objectRadius / 2);
        objectTransform.position -= Vector3.back * dist;
    }
}
