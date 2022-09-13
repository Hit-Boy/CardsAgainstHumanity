using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignToEarthRotation : MonoBehaviour
{
    void Start()
    {
        Transform objectTransform = GetComponent<Transform>();
        Transform earthTf = GameObject.FindGameObjectWithTag("Earth").transform;
        Vector3 delta = (objectTransform.position - earthTf.position).normalized;
        objectTransform.rotation = Quaternion.LookRotation(delta, Vector3.up);
        objectTransform.Rotate(90, 0, 0);
    }
}
