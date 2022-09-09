using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class World : MonoBehaviour {
    [SerializeField] private TMP_Text resourceText;
    [SerializeField] private TMP_Text expiredCardsText;
    [SerializeField] private TMP_Text looseText;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private GameObject hand;
    [SerializeField]
    private int maxExpiredCards = 3;
    [SerializeField]
    private int firstCardTime = 2;

    [SerializeField] private float cardCooldown = 3;
    
    private Hand handScript;

    private string resourceTextValue;
    private string expiredCardsTextValue;

    public int numberOfExpiredCards = 0;

    private int environment = 50;
    private int people = 50;
    private int energy = 50;
    private int money = 50;
    private int time = 2022;
    private float startingTime;
    private float currentTime;
    
    
    
    // Start is called before the first frame update
    void Start() {
        startingTime = Time.time - (10-firstCardTime);
        handScript = hand.GetComponent<Hand>();
        UpdateResourceTextValue();
        UpdateExpiredCardsNumber();
    }

    // Update is called once per frame
    void Update() {
        CountWorldTime();
        CheckExpiredCards();
        CheckResources();
    }

    private void CountWorldTime() {
        if ((Time.time - startingTime) / cardCooldown >= 1) {
            handScript.DrawCard();
            startingTime = Time.time;
        }
    }
    
    public void UpdateResourceTextValue() {
        resourceTextValue = "Env: " + environment + "  Peo: " + people + "  Ene: " + energy + "  Mon: " + money;
        resourceText.text = resourceTextValue;
    }

    public void UpdateExpiredCardsNumber() {
        expiredCardsTextValue = numberOfExpiredCards + "/" + maxExpiredCards;
        expiredCardsText.text = expiredCardsTextValue;
    }

    private void CheckExpiredCards() {
        if (numberOfExpiredCards == maxExpiredCards) looseText.gameObject.SetActive(true);
        }

    public void AddToResources(int env, int peo, int ene, int mon) {
        environment += env;
        people += peo;
        energy += ene;
        money += mon;
        UpdateResourceTextValue();
    }

    private void CheckResources() {
        if (environment <= 0 || people <= 0 || energy <= 0 || money <= 0) looseText.gameObject.SetActive(true);
    }

    public void ActivateWinScreen() {
        winText.gameObject.SetActive(true);
    }
}
