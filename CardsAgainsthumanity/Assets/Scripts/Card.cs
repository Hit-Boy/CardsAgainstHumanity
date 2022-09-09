using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using Palmmedia.ReportGenerator.Core;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour {
    
    [SerializeField] private float maxLifeTime = 10;
    //[SerializeField] private GameObject world;
    //[SerializeField] private GameObject hand;
    [SerializeField] private TMP_Text nameText;
    private World worldScript;
    private Hand handScript;
    private float startLifeTime;
    public bool isSelected;
    public bool isHoveredOver;

    [Header("FirstChoiceConsequences(Yes)")] 
    [SerializeField]
    private int firstEnvCon;
    [SerializeField]
    private int firstPeoCon;
    [SerializeField]
    private int firstEneCon;
    [SerializeField]
    private int firstMonCon;
    
    [Header("SecondChoiceConsequences(No)")] 
    [SerializeField]
    private int secondEnvCon;
    [SerializeField]
    private int secondPeoCon;
    [SerializeField]
    private int secondEneCon;
    [SerializeField]
    private int secondMonCon;

// Start is called before the first frame update
    void Start() {
        worldScript = GameObject.FindWithTag("World").GetComponent<World>();
        handScript = GameObject.FindWithTag("Hand").GetComponent<Hand>();
        SetName();
    }

    // Update is called once per frame
    void Update() {
        CheckIfTimeExpired();
    }

    private void SetName() {
        nameText.text = name;
    }

    public bool CheckIfTimeExpired() {
        return Time.time - startLifeTime >= maxLifeTime;
    }

    public void SetStartLifeTime(float time) {
        startLifeTime = time;
    }

    public void Scale() {
        if (isSelected) return;
        Debug.Log(transform.position);
        transform.localScale *= 1.5f;
        transform.position += new Vector3(0f, 45f, 0f);
        isHoveredOver = true;
    }

    public void ScaleBack() {
        if(isSelected) return;
        transform.localScale /= 1.5f;
        transform.position -= new Vector3(0f, 45f, 0f);
        isHoveredOver = false;
    }

    public void SelectCard() {
        if (isSelected == false) {
            transform.localScale *= 1.5f;
            transform.position += new Vector3(0f, 67.5f, 0f);
            transform.Find("FirstChoice").gameObject.SetActive(true);
            transform.Find("SecondChoice").gameObject.SetActive(true);
            isSelected = true;
        }
        else {
            transform.localScale /= 1.5f;
            transform.position -= new Vector3(0f, 67.5f, 0f);
            transform.Find("FirstChoice").gameObject.SetActive(false);
            transform.Find("SecondChoice").gameObject.SetActive(false);
            isSelected = false;
        }
    }

    public void MakeFirstChoice() {
        int index = -500;
        worldScript.AddToResources(firstEnvCon, firstPeoCon, firstEneCon, firstMonCon);
        
        for (int i = 0; i < handScript.handCards.Count; i++) {
            if (gameObject == handScript.handCards[i]) index = i;
        }
        handScript.DiscardCard(index);
    }

    public void MakeSecondChoice() {
        int index = -500;
        worldScript.AddToResources(secondEnvCon, secondPeoCon, secondEneCon, secondMonCon);
        for (int i = 0; i < handScript.handCards.Count; i++) {
            if (gameObject == handScript.handCards[i]) index = i;
        }
        handScript.DiscardCard(index);
    }

    
}
