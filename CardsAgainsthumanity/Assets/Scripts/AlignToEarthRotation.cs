
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
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    Transform child = transform.GetChild(i);
                    child.GetComponent<MeshRenderer>().enabled = false;
                    if (i == 0)
                    {
                        for (int j = child.transform.childCount - 1; j >= 0; --j)
                        {
                            Transform childd = child.transform.GetChild(j);
                            childd.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                }
                GetComponent<MeshRenderer>().enabled = false;
                break;
            case "NuclearUS":
                objectHeigt = 4.5f;
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    Transform child = transform.GetChild(i);
                    child.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case "Nuclear":
                objectHeigt = 4.4f;
                break;
            case "Rocket":
                objectHeigt = 4;
                break;
            case "WaterPlant":
                objectHeigt = 4.8f;
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    Transform child = transform.GetChild(i);
                    child.GetComponent<MeshRenderer>().enabled = false;
                }
                break;
            case "Crane":
                objectHeigt = 4.3f;
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    Transform child = transform.GetChild(i);
                    child.GetComponent<MeshRenderer>().enabled = false;
                    if (i == 0)
                    {
                        for (int j = transform.childCount - 1; j >= 0; --j)
                        {
                            Transform childd = child.transform.GetChild(j);
                            childd.GetComponent<MeshRenderer>().enabled = false;
                        }
                    }
                }
                break;
            case "Tree1":
                objectHeigt = 4.5f;
                break;
            case "House1":
                objectHeigt = 4.4f;
                break;
            case "TreeAmazon":
            case "Tree2":
                objectHeigt = 4.2f;
                break;
            case "City":
                objectHeigt = 4;
                break;
            case "Farm":
                objectHeigt = 4;
                for (int i = transform.childCount - 1; i >= 0; --i)
                {
                    Transform child = transform.GetChild(i);
                    child.GetComponent<MeshRenderer>().enabled = false;
                }
                GetComponent<MeshRenderer>().enabled = false;
                break;
        }

        Transform objectTransform = GetComponent<Transform>();
        Transform earthTf = GameObject.FindGameObjectWithTag("Earth").transform;
        Vector3 delta = objectTransform.position - earthTf.position;

        objectTransform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
        objectTransform.Rotate(90, 0, 0);

        float dist = delta.magnitude - earthRadius - objectHeigt / 2;
        objectTransform.position -= objectTransform.up * dist;

        if (tag != "Crane")
        {
            objectTransform.Rotate(0, Random.Range(0, 360), 0);
        }
        if (tag == "Turbine")
        {
            objectTransform.Rotate(180,0, 0);
        }
    }
}