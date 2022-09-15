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
    [SerializeField] private TMP_Text timerText;
    private World worldScript;
    private Hand handScript;
    private RotateEarth earthScript;
    private float startLifeTime;
    public bool isSelected;
    public bool isHoveredOver;
    public Vector3 cardPosition;
    public int cardHandPosition;
    private Vector3 centerPosition;

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
        earthScript = GameObject.FindWithTag("Earth").GetComponent<RotateEarth>();
        worldScript = GameObject.FindWithTag("World").GetComponent<World>();
        handScript = GameObject.FindWithTag("Hand").GetComponent<Hand>();
        Rect canvasRect = GameObject.FindWithTag("Canvas").GetComponent<RectTransform>().rect;
        centerPosition = new Vector3(canvasRect.width/2, canvasRect.height/3, 0);
        SetName();
    }

    // Update is called once per frame
    void Update() {
        UpdateTimer();
    }

    private void SetName() {
        nameText.text = name;
    }

    public bool CheckIfTimeExpired() {
        return Time.time - startLifeTime >= maxLifeTime;
    }
    
    private float RemainingTime() {
        return Time.time - startLifeTime;
    }

    private void UpdateTimer() {
        timerText.text = Mathf.Ceil(maxLifeTime - RemainingTime()).ToString();
    }

    public void SetStartLifeTime(float time) {
        startLifeTime = time;
    }

    public void ScaleToHoverSize() {
        if (earthScript.IsDragging()) return;
        if (isSelected) return;
        transform.localScale = new Vector3(1.5f,1.5f, 1.5f);
        transform.position = cardPosition + new Vector3(0f, 90f, 0f);
        //transform.GetSiblingIndex();
        transform.SetSiblingIndex(10);
        isHoveredOver = true;
    }

    public void ScaleToNormalSize() {
        if (earthScript.IsDragging()) return;
        if(isSelected) return;
        transform.localScale = new Vector3(1f,1f, 1f);
        transform.position = cardPosition;
        transform.SetSiblingIndex(cardHandPosition);
        isHoveredOver = false;
    }

    public void SelectCard() {
        if (earthScript.IsDragging()) return;
        if (isSelected == false) {
            if (handScript.numberOfSelectedCards > 0) return;
            transform.localScale = new Vector3(2f,2f, 2f);
            transform.position = centerPosition;
            //transform.position = cardPosition + new Vector3(0f, 225f, 0f);
            EnableButtons(true);
            isSelected = true;
            handScript.numberOfSelectedCards++;
        }
        else {
            transform.localScale = new Vector3(1.5f,1.5f, 1.5f);
            transform.position = cardPosition + new Vector3(0f, 90f, 0f);
            EnableButtons(false);
            isSelected = false;
            handScript.numberOfSelectedCards--;
        }
    }

    public void SetPosition(Vector3 pos) {
        cardPosition = pos;
        if (isSelected) 
            transform.position = centerPosition;
        else {
            if(isHoveredOver)
                transform.position = cardPosition + new Vector3(0f, 90f, 0f);
            else 
                transform.position = cardPosition;
        }
    }

    private void EnableButtons(bool activity) {
        transform.Find("FirstChoice").gameObject.SetActive(activity);
        transform.Find("SecondChoice").gameObject.SetActive(activity);
    }

    public void MakeFirstChoice() {
        if (worldScript.isPaused) return;
        int index = -500;
        worldScript.AddToResources(firstEnvCon, firstPeoCon, firstEneCon, firstMonCon);
        
        for (int i = 0; i < handScript.handCards.Count; i++) {
            if (gameObject == handScript.handCards[i]) index = i;
        }
        handScript.DiscardCard(index);
    }

    public void MakeSecondChoice() {
        if (worldScript.isPaused) return;
        int index = -500;
        worldScript.AddToResources(secondEnvCon, secondPeoCon, secondEneCon, secondMonCon);
        for (int i = 0; i < handScript.handCards.Count; i++) {
            if (gameObject == handScript.handCards[i]) index = i;
        }
        handScript.DiscardCard(index);
    }
}
