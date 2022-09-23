using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

public class Card : MonoBehaviour
{

    [SerializeField] private float maxLifeTime = 10;
    //[SerializeField] private GameObject world;
    //[SerializeField] private GameObject hand;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float cardMoveSpeed = 150f;
    [SerializeField] private float HoverMoveSpeed = 50f;
    [SerializeField] private float SelectMoveSpeed = 80f;

    private Deck deckScript;
    private World worldScript;
    private Hand handScript;
    private RotateEarth earthScript;
    private TMP_Text newsText;
    private float startLifeTime;
    public bool isSelected;
    public bool isHoveredOver;
    private bool isMovingToCardPosition = true;
    public Vector3 cardPosition;
    private Vector3 desiredCardPosition;
    public int cardHandPosition;
    private Vector3 centerPosition;
    AudioSource soundEffect;

    [Header("FirstChoiceConsequences(Yes)")]
    [SerializeField]
    private int firstEnvCon;
    [SerializeField]
    private int firstPeoCon;
    [SerializeField]
    private int firstEneCon;
    [SerializeField]
    private int firstMonCon;
    [SerializeField]
    private string firstNews;
    [SerializeField]
    private GameObject firstCard;
    [SerializeField]
    private UnityEvent firstFunction;

    [Header("SecondChoiceConsequences(No)")]
    [SerializeField]
    private int secondEnvCon;
    [SerializeField]
    private int secondPeoCon;
    [SerializeField]
    private int secondEneCon;
    [SerializeField]
    private int secondMonCon;
    [SerializeField]
    private string secondNews;
    [SerializeField]
    private GameObject secondCard;
    [SerializeField]
    private UnityEvent secondFunction;

    // Start is called before the first frame update
    void Start()
    {
        earthScript = GameObject.FindWithTag("World").GetComponent<RotateEarth>();
        deckScript = GameObject.FindWithTag("Deck").GetComponent<Deck>();
        worldScript = GameObject.FindWithTag("World").GetComponent<World>();
        handScript = GameObject.FindWithTag("Hand").GetComponent<Hand>();
        newsText = GameObject.FindWithTag("NewsText").GetComponent<TMP_Text>();
        soundEffect = GetComponent<AudioSource>();
        Rect canvasRect = GameObject.FindWithTag("Canvas").GetComponent<RectTransform>().rect;
        centerPosition = new Vector3(canvasRect.width / 5, canvasRect.height * 3 / 8, 0);
        SetName();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
        MoveToCardPosition();
    }

    private void SetName()
    {
        nameText.text = name;
    }

    public bool CheckIfTimeExpired()
    {
        return Time.time - startLifeTime >= maxLifeTime;
    }

    private float RemainingTime()
    {
        return Time.time - startLifeTime;
    }

    private void UpdateTimer()
    {
        timerText.text = Mathf.Ceil(maxLifeTime - RemainingTime()).ToString();
    }

    public void SetStartLifeTime(float time)
    {
        startLifeTime = time;
    }

    public void ScaleToHoverSize()
    {
        if (isMovingToCardPosition) return;
        if (earthScript.IsDragging()) return;
        if (isSelected) return;
        transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        //transform.position = cardPosition + new Vector3(0f, 90f, 0f);
        desiredCardPosition = cardPosition + new Vector3(0f, 90f, 0f);
        transform.SetSiblingIndex(10);
        isHoveredOver = true;
    }

    public void ScaleToNormalSize()
    {
        // if (isMovingToCardPosition) return;
        if (earthScript.IsDragging()) return;
        if (isSelected) return;
        transform.localScale = new Vector3(1f, 1f, 1f);
        desiredCardPosition = cardPosition;
        transform.SetSiblingIndex(cardHandPosition);
        isHoveredOver = false;
    }

    public void SelectCard()
    {
        if (isMovingToCardPosition) return;
        if (earthScript.IsDragging()) return;
        transform.SetSiblingIndex(20);
        if (isSelected == false)
        {
            if (handScript.numberOfSelectedCards > 0) return;
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            desiredCardPosition = centerPosition;
            //transform.position = cardPosition + new Vector3(0f, 225f, 0f);
            EnableButtons(true);
            isSelected = true;
            handScript.numberOfSelectedCards++;
        }
        else
        {
            transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            //transform.position = cardPosition + new Vector3(0f, 90f, 0f);
            desiredCardPosition = cardPosition + new Vector3(0f, 90f, 0f);
            transform.SetSiblingIndex(cardHandPosition);
            EnableButtons(false);
            isSelected = false;
            handScript.numberOfSelectedCards--;
        }
    }

    public void SetDesiredPosition(Vector3 pos)
    {
        SetCardPosition(pos);
        if (isSelected)
            desiredCardPosition = centerPosition;
        else
        {
            if (isHoveredOver)
                desiredCardPosition = cardPosition + new Vector3(0f, 90f, 0f);
            else
                desiredCardPosition = cardPosition;
        }
    }

    public void SetCardPosition(Vector3 pos)
    {
        cardPosition = pos;
    }

    private void EnableButtons(bool activity)
    {
        transform.Find("FirstChoice").gameObject.SetActive(activity);
        transform.Find("SecondChoice").gameObject.SetActive(activity);
    }

    public void MakeFirstChoice()
    {
        if (worldScript.isPaused) return;
        int index = -500;
        worldScript.AddToResources(firstEnvCon, firstPeoCon, firstEneCon, firstMonCon);
        soundEffect.Play();

        for (int i = 0; i < handScript.handCards.Count; i++)
        {
            if (gameObject == handScript.handCards[i]) index = i;
        }
        handScript.numberOfSelectedCards--;
        handScript.DiscardCard(index);
        PostNews(firstNews);
        AddCardToTheDeck(secondCard);
        CallOtherFunction(firstFunction);
    }

    public void MakeSecondChoice()
    {
        if (worldScript.isPaused) return;
        int index = -500;
        worldScript.AddToResources(secondEnvCon, secondPeoCon, secondEneCon, secondMonCon);
        soundEffect.Play();

        for (int i = 0; i < handScript.handCards.Count; i++)
        {
            if (gameObject == handScript.handCards[i]) index = i;
        }
        handScript.numberOfSelectedCards--;
        handScript.DiscardCard(index);
        PostNews(secondNews);
        AddCardToTheDeck(secondCard);
        CallOtherFunction(secondFunction);
    }

    private void PostNews(string news)
    {
        if (news == "") return;
        newsText.text = news + "\n" + newsText.text;
    }

    private void MoveToCardPosition()
    {
        if (transform.position == desiredCardPosition) return;
        float MoveSpeed = cardMoveSpeed;
        if (isSelected)
            MoveSpeed = SelectMoveSpeed;
        if (isHoveredOver)
            MoveSpeed = SelectMoveSpeed;



        //transform.position = desiredCardPosition;
        //isMovingToCardPosition = false;
        Vector3 moveVector = desiredCardPosition - transform.position;
        //Debug.Log(moveVector.magnitude < cardMoveSpeed * Time.unscaledDeltaTime);
        if (moveVector.magnitude < MoveSpeed * Time.unscaledDeltaTime)
        {
            transform.position = desiredCardPosition;
            isMovingToCardPosition = false;
            return;
        }

        transform.position += moveVector.normalized * MoveSpeed * Time.unscaledDeltaTime;
        isMovingToCardPosition = true;
    }

    private void AddCardToTheDeck(GameObject card)
    {
        if (card == null) return;
        deckScript.AddCard(card);
    }

    private void CallOtherFunction(UnityEvent function)
    {
        if (function == null) return;
        function.Invoke();
    }
}
