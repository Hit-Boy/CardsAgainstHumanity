using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToEarthRotation : MonoBehaviour
{
    float earthRadius = 5;
    float objectRadius = 5;

    void Start()
    {
        //float earthRadius = GameObject.FindGameObjectWithTag("Earth").GetComponent<MeshFilter>().mesh.bounds.extents.x;
        //float objectRadius = GetComponent<MeshFilter>().mesh.bounds.extents.x;

        Transform objectTransform = GetComponent<Transform>();
        Transform earthTf = GameObject.FindGameObjectWithTag("Earth").transform;
        Vector3 delta = (objectTransform.position - earthTf.position).normalized;
        objectTransform.rotation = Quaternion.LookRotation(delta, Vector3.up);
        objectTransform.Rotate(90, 0, 0);

        float dist = (objectTransform.position - earthTf.position).magnitude - earthRadius / 2 - objectRadius / 2;
        objectTransform.position -= Vector3.back * dist;
    }
}
