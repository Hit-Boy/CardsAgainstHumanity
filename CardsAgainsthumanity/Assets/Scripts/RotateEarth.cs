
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
    public int minZoom = -35;
    [SerializeField]
    public int maxZoom = 50;

    private bool isDragging = false;
    private Hand handScript;
    Transform earthTf;

    void Start()
    {
        handScript = GameObject.FindWithTag("Hand").GetComponent<Hand>();
        earthTf = GameObject.FindWithTag("Earth").GetComponent<Transform>();
    }

    void Update()
    {
        CheckForModelSwitch();
        Dragging();
        Scrolling();
        Debug();
    }

    void CheckForModelSwitch()
    {
        if (earthTf == null)
        {
            earthTf = GameObject.FindWithTag("Earth").GetComponent<Transform>();
        }
    }

    void Debug()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CardEffectUs();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CardEffectAmazon();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            CardEffectChina();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            CardEffectFarms();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            CardEffectItaly();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            CardEffectTurbine();
        }
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

    public void CardEffectAmazon()
    {
        foreach (GameObject amazonTree in GameObject.FindGameObjectsWithTag("TreeAmazon"))
        {
            for (int i = amazonTree.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = amazonTree.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    public void CardEffectUs()
    {
        foreach (GameObject usPlant in GameObject.FindGameObjectsWithTag("NuclearUS"))
        {
            for (int i = usPlant.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = usPlant.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void CardEffectChina()
    {
        foreach (GameObject damChina in GameObject.FindGameObjectsWithTag("WaterPlant"))
        {
            for (int i = damChina.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = damChina.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    public void CardEffectFarms()
    {
        foreach (GameObject farm in GameObject.FindGameObjectsWithTag("Farm"))
        {
            for (int i = farm.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = farm.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
            farm.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void CardEffectItaly()
    {
        foreach (GameObject crane in GameObject.FindGameObjectsWithTag("Crane"))
        {
            for (int i = crane.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = crane.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
                if (i == 0)
                {
                    for (int j = crane.transform.childCount - 1; j >= 0; --j)
                    {
                        Transform childd = child.transform.GetChild(j);
                        childd.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    public void CardEffectTurbine()
    {
        foreach (GameObject turbine in GameObject.FindGameObjectsWithTag("Turbine"))
        {
            for (int i = turbine.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = turbine.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
                if (i == 0)
                {
                    for (int j = child.transform.childCount - 1; j >= 0; --j)
                    {
                        Transform childd = child.transform.GetChild(j);
                        childd.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
            turbine.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
