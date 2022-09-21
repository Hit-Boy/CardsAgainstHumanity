using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {
    private Slider slider;
    // Start is called before the first frame update


    private void Awake() {
        slider = gameObject.GetComponent<Slider>();
    }

    void Start()
    {
        
        SetValue(50);
        //slider.maxValue = 100;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SetValue(int passedValue) {
        slider.value = passedValue;
    }
    

}
