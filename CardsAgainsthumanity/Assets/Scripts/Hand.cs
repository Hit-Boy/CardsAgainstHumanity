using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEditor.Experimental;
using UnityEngine;
using Random = System.Random;

public class Hand : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> handCards;
    private List<Card> cardScripts;
    public List<Vector3> cardPositions;
    [SerializeField] private GameObject deck;
    private GameObject canvas;
    private World worldScript;
    private Deck deckScript;
    private DiscardPile discardScript;
    public int numberOfSelectedCards = 0;
    void Start() {

        handCards = new List<GameObject>();
        cardScripts = new List<Card>();
        canvas = GameObject.Find("Canvas");
        worldScript = GameObject.Find("WorldManager").GetComponent<World>();
        deckScript = deck.GetComponent<Deck>();
        discardScript = GameObject.FindWithTag("DiscardPile").GetComponent<DiscardPile>();
        cardPositions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update() {
        CheckOldestCard();
    }
    
    public void DrawCard() {
        if (deckScript.deckCards.Count == 0) {
            worldScript.ActivateWinScreen();
            return;
        }

        GameObject card = deckScript.deckCards[deckScript.deckCards.Count - 1];
        deckScript.deckCards.RemoveAt(deckScript.deckCards.Count - 1);
        AddCard(card);
        UpdateCardPositions();
        card.transform.position = new Vector3(2000, 200, 0);
    }

    private void AddCard(GameObject card) { 
        Card cardScript = card.GetComponent<Card>();
        handCards.Add(card);
        card.transform.SetParent(transform);
        card.transform.SetSiblingIndex(0);
        cardScripts.Add(cardScript);
        cardScript.SetStartLifeTime(Time.time);
        cardScript.cardHandPosition = handCards.Count - 1;
        card.SetActive(true);
    }

    private void CheckOldestCard() {
        if (handCards.Count == 0) return;
        Card cardScript = handCards[0].GetComponent<Card>();
        if (cardScript.CheckIfTimeExpired()) {
            DiscardCard(0);
            worldScript.numberOfExpiredCards++;
            worldScript.UpdateExpiredCardsNumber();
        }
    }

    public void DiscardCard(int index) {
        GameObject card = handCards[index];
        discardScript.discardedCards.Add(card.name);
        handCards.RemoveAt(index);
        cardScripts.RemoveAt(index);
        Destroy(card);
        UpdateCardPositions();
        for (int i = 0; i < cardScripts.Count; i++) {
            if (index - 1 < i) {
                cardScripts[i].cardHandPosition--;
            }
        }
    }

    private void CreateCardPositions() {
        cardPositions.Clear();
        int positionCount = handCards.Count;
        Rect handRect = gameObject.GetComponent<RectTransform>().rect;
        //320 = 2 buttons width + 20 
        //Vector3 positionShift = new Vector3((handRect.width - 320) / (positionCount + 1), 0f, 0f);
        Vector3 positionShift = new Vector3(180f, 0f, 0f);
        Vector3 startPosition = new Vector3(-(handRect.width/2 - 160) - handRect.x, 50 - handRect.y, 0);
        for (int i = 0; i < positionCount; i++) {
            cardPositions.Add(startPosition + positionShift * (i + 1));
        }
    }

    public bool CheckHoveringOfHandCards() {
        bool isHoveringOverHandCards = false;
        foreach (Card cardScript in cardScripts) {
            if (cardScript.isSelected || cardScript.isHoveredOver)
                isHoveringOverHandCards = true;
        }
        return isHoveringOverHandCards;
    }

    private void SetCardsIntoPositions() {
        for (int i = 0; i < handCards.Count; i++) {
            cardScripts[i].SetDesiredPosition(cardPositions[i]);
        }
    }

    private void UpdateCardPositions() {
        CreateCardPositions();
        SetCardsIntoPositions();
    }
}
