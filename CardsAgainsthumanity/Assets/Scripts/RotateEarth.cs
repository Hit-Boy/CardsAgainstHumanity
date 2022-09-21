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

    public GameObject wetEarth;
    public GameObject dryEarth;
    public GameObject normalEarth;

    private bool isDragging = false;
    private Hand handScript;
    Transform earthTf;

    void Start()
    {
        handScript = GameObject.FindWithTag("Hand").GetComponent<Hand>();
        earthTf = GetComponent<Transform>();
    }

    void Update()
    {
        Dragging();
        Scrolling();
        Debug();
    }

    void Debug()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchEarthModel(wetEarth, "Earth");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchEarthModel(dryEarth, "WetEarth");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchEarthModel(wetEarth, "DryEarth");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchEarthModel(normalEarth, "WetEarth");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            CardEffectUs();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CardEffectAmazon();
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




    void SwitchEarthModel(GameObject newEarthModel, string oldEarthModelTag)
    {
        GameObject oldEarth = GameObject.FindWithTag(oldEarthModelTag);
        GameObject newEarth = Instantiate(newEarthModel, oldEarth.transform.position, oldEarth.transform.rotation);
        for (int i = oldEarth.transform.childCount - 1; i >= 0; --i)
        {
            Transform child = oldEarth.transform.GetChild(i);
            if (child.name.Contains("Mountains") || child.name.Contains("Land"))
            {
                continue;
            }
            child.SetParent(newEarth.transform, false);
        }
        Destroy(oldEarth);
    }

    void CardEffectAmazon()
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

    void CardEffectUs()
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

    void CardEffectChina()
    {
        foreach (GameObject damChina in GameObject.FindGameObjectsWithTag("DamChina"))
        {
            for (int i = damChina.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = damChina.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void CardEffectRandomNuclear()
    {
        foreach (GameObject nuclearPlant in GameObject.FindGameObjectsWithTag("RandomNuclear"))
        {
            for (int i = nuclearPlant.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = nuclearPlant.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void CardEffectFarms()
    {
        foreach (GameObject farm in GameObject.FindGameObjectsWithTag("RandomFarm"))
        {
            for (int i = farm.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = farm.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void CardEffectItaly()
    {
        foreach (GameObject crane in GameObject.FindGameObjectsWithTag("ItalyCrane"))
        {
            for (int i = crane.transform.childCount - 1; i >= 0; --i)
            {
                Transform child = crane.transform.GetChild(i);
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    void CardEffectTurbine()
    {

    }
}
