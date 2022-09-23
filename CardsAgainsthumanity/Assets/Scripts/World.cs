using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using UnityEngine.SceneManagement;


public class World : MonoBehaviour
{
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

    private int environment = 80;
    private int people = 50;
    private int energy = 50;
    private int money = 50;
    private int time = 2022;
    private float startingTime;
    private float currentTime;
    private bool wetEarthActivated = false;
    private bool dryEarthActivated = false;
    private int envUpperBorder = 50;
    private int envLowerBorder = 25;

    // Start is called before the first frame update
    void Start()
    {

        newsText = GameObject.FindWithTag("NewsText").GetComponent<TMP_Text>();
        startingTime = Time.time + firstCardTime - cardCooldown;
        handScript = hand.GetComponent<Hand>();
        UpdateResourceTextValue();
        UpdateExpiredCardsNumber();
        UpdateResourceBars();
        CheckAllThresholds();
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("EndScreen");
    }

    // Update is called once per frame
    void Update()
    {
        CountWorldTime();
        CheckExpiredCards();
        CheckResources();
        CheckSwictEarthModel();       
        Debug();
    }

    void Debug()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchEarthModel((GameObject)Resources.Load("EarthWetV2", typeof(GameObject)), "Earth");
            wetEarthActivated = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Transform oldEarth = GameObject.FindGameObjectWithTag("Earth").transform;
            for (int i = oldEarth.childCount - 1; i >= 0; --i)
            {
                GameObject child = oldEarth.GetChild(i).gameObject;
                if (child.tag == "Tree1" || child.tag == "Tree2")
                {
                    Destroy(child);
                }
            }
            SwitchEarthModel((GameObject)Resources.Load("EarthDryV2", typeof(GameObject)), "Earth");
            dryEarthActivated = true;
        }
    }

    private void CountWorldTime()
    {
        if ((Time.time - startingTime) / cardCooldown >= 1)
        {
            handScript.DrawCard();
            startingTime = Time.time;
        }
    }

    public void UpdateResourceTextValue()
    {
        resourceTextValue = "Env: " + environment + "  Peo: " + people + "  Ene: " + energy + "  Mon: " + money;
        resourceText.text = resourceTextValue;
    }

    public void UpdateExpiredCardsNumber()
    {
        expiredCardsTextValue = numberOfExpiredCards + "/" + maxExpiredCards;
        expiredCardsText.text = expiredCardsTextValue;
    }

    private void CheckExpiredCards()
    {
        if (numberOfExpiredCards == maxExpiredCards)
        {
            looseText.gameObject.SetActive(true);
            StartCoroutine(waiter());
        }
    }

    public void AddToResources(int env, int peo, int ene, int mon)
    {
        environment += env;
        people += peo;
        energy += ene;
        money += mon;
        UpdateResourceTextValue();
        UpdateResourceBars();
        CheckAllThresholds();
    }

    private void CheckResources()
    {
        if (environment <= 0 || people <= 0 || energy <= 0 || money <= 0)
        {
            looseText.gameObject.SetActive(true);
            StartCoroutine(waiter());
        }
    }

    public void ActivateWinScreen()
    {
        winText.gameObject.SetActive(true);
    }

    public void ChangePauseState()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            pauseText.text = "Pause";
        }
        else
        {
            Time.timeScale = 0;
            pauseText.text = "Unpause";
        }
        isPaused = !isPaused;
    }

    private void UpdateResourceBars()
    {
        envBar.SetValue(environment);
        peopleBar.SetValue(people);
        energyBar.SetValue(energy);
        moneyBar.SetValue(money);
    }

    private void PostNews(string news)
    {
        if (news == "") return;
        newsText.text = news + "\n" + newsText.text;
    }

    private void CheckAllThresholds()
    {
        CheckParticularThresholds(environmentThreshold, environmentNews);
        CheckParticularThresholds(peopleThreshold, peopleNews);
        CheckParticularThresholds(energyThreshold, energyNews);
        CheckParticularThresholds(moneyThreshold, moneyNews);
    }


    private void CheckParticularThresholds(List<int> thresholdList, List<string> newsList)
    {
        for (int i = 0; i < thresholdList.Count; i++)
        {
            if (environment <= thresholdList[i])
            {
                PostNews(newsList[i]);
                newsList.RemoveAt(i);
                thresholdList.RemoveAt(i);
            }
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

    void CheckSwictEarthModel()
    {
        if (!wetEarthActivated && environment < envUpperBorder && environment > envLowerBorder)
        {
            SwitchEarthModel((GameObject)Resources.Load("EarthWetV2", typeof(GameObject)), "Earth");
            wetEarthActivated = true;
        }
        else if (!dryEarthActivated && environment < envLowerBorder)
        {
            Transform oldEarth = GameObject.FindGameObjectWithTag("Earth").transform;
            for (int i = oldEarth.childCount - 1; i >= 0; --i)
            {
                GameObject child = oldEarth.GetChild(i).gameObject;
                if (child.tag == "Tree1" || child.tag == "Tree2" || child.tag == "TreeAmazon")
                {
                    Destroy(child);
                }
            }
            SwitchEarthModel((GameObject)Resources.Load("EarthDryV2", typeof(GameObject)), "Earth");
            dryEarthActivated = true;
        }
    }

}
