using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTurbines : MonoBehaviour
{
    GameObject[] turbineCenters;
    [SerializeField]
    public int rotationSpeed = 5;

    void Start()
    {
        turbineCenters = GameObject.FindGameObjectsWithTag("TurbineCenter");
    }

    void Update()
    {
        foreach (GameObject turbineCenter in turbineCenters)
        {
            turbineCenter.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }   
    }
}
