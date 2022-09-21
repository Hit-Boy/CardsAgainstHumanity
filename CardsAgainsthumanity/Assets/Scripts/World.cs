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
    [SerializeField] private TMP_Text pauseText;

    [SerializeField] private SliderScript envBar;
    [SerializeField] private SliderScript peopleBar;
    [SerializeField] private SliderScript energyBar;
    [SerializeField] private SliderScript moneyBar;
    
    [SerializeField] private GameObject hand;
    [SerializeField]
    private int maxExpiredCards = 3;
    [SerializeField]
    private int firstCardTime = 2;

    [SerializeField] private float cardCooldown = 3;

    public bool isPaused = false;
    public int numberOfExpiredCards = 0;
    
    [Header("News texts and thresholds")] 
    [SerializeField]
    private List<int> environmentThreshold;
    [SerializeField]
    private List<string> environmentNews;
    [SerializeField]
    private List<int> peopleThreshold;
    [SerializeField]
    private List<string> peopleNews;
    [SerializeField]
    private List<int> energyThreshold;
    [SerializeField]
    private List<string> energyNews;
    [SerializeField]
    private List<int> moneyThreshold;
    [SerializeField]
    private List<string> moneyNews;
    
    
    
    private Hand handScript;
    private TMP_Text newsText;
    private string resourceTextValue;
    private string expiredCardsTextValue;
    
    private int environment = 50;
    private int people = 50;
    private int energy = 50;
    private int money = 50;
    private int time = 2022;
    private float startingTime;
    private float currentTime;
    
    // Start is called before the first frame update
    void Start() {

        newsText = GameObject.FindWithTag("NewsText").GetComponent<TMP_Text>();
        startingTime = Time.time + firstCardTime - cardCooldown;
        Debug.Log(Time.time);
        Debug.Log(Time.time - startingTime);
        handScript = hand.GetComponent<Hand>();
        UpdateResourceTextValue();
        UpdateExpiredCardsNumber();
        UpdateResourceBars();
        CheckAllThresholds();
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
        UpdateResourceBars();
        CheckAllThresholds();
    }

    private void CheckResources() {
        if (environment <= 0 || people <= 0 || energy <= 0 || money <= 0) looseText.gameObject.SetActive(true);
    }

    public void ActivateWinScreen() {
        winText.gameObject.SetActive(true);
    }
    
    public void ChangePauseState() {
        if (isPaused) {
            Time.timeScale = 1;
            pauseText.text = "Pause";
        }
        else {
            Time.timeScale = 0;
            pauseText.text = "Unpause";
        }
        isPaused = !isPaused;
    }

    private void UpdateResourceBars() {
        envBar.SetValue(environment);
        peopleBar.SetValue(people);
        energyBar.SetValue(energy);
        moneyBar.SetValue(money);
    }
    
    private void PostNews(string news) {
        if(news == "") return;
        newsText.text = news + "\n" + newsText.text;
    }

    private void CheckAllThresholds() {
        CheckParticularThresholds(environmentThreshold, environmentNews);
        CheckParticularThresholds(peopleThreshold, peopleNews);
        CheckParticularThresholds(energyThreshold, energyNews);
        CheckParticularThresholds(moneyThreshold, moneyNews);
    }


    private void CheckParticularThresholds(List<int> thresholdList, List<string> newsList) {
        for (int i = 0; i < thresholdList.Count; i++) {
            if (environment <= thresholdList[i]) {
                PostNews(newsList[i]);
                newsList.RemoveAt(i);
                thresholdList.RemoveAt(i);
            }
        }
    }




}
