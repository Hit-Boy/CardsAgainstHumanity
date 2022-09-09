using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> handCards;
    private List<Vector3> cardPositions;
    [SerializeField] private GameObject deck;
    private GameObject canvas;
    private World worldScript;
    private Deck deckScript;
    private DiscardPile discardScript;
    void Start() {
        handCards = new List<GameObject>();
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
        }

        GameObject card = deckScript.deckCards[deckScript.deckCards.Count - 1];
        card.transform.SetParent(transform);
        handCards.Add(card);
        deckScript.deckCards.RemoveAt(deckScript.deckCards.Count - 1);
        Card cardScript = card.GetComponent<Card>();
        cardScript.SetStartLifeTime(Time.time);
        UpdateCardPositions();
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
        Destroy(card);
        UpdateCardPositions();
    }

    private void CreateCardPositions() {
        cardPositions.Clear();
        int positionCount = handCards.Count;
        Rect canvasRect = canvas.GetComponent<RectTransform>().rect;
        //160 = 2 buttons width + 20 
        Vector3 positionShift = new Vector3((canvasRect.width - 160) / (positionCount + 1), 0f, 0f);
        Vector3 startPosition = new Vector3(-(canvasRect.width/2 - 80) - canvasRect.x, -150 - canvasRect.y, 0);
        for (int i = 0; i < positionCount; i++) {
            float selectionYShift = 0;
            Card cardScript = handCards[i].GetComponent<Card>();
            // numbers are from shifts on selecting and hovering over of cards
            if (handCards[i].GetComponent<Card>().isSelected) selectionYShift = 112.5f;  
            else if (handCards[i].GetComponent<Card>().isHoveredOver) selectionYShift = 45f; // can be very buggy
            cardPositions.Add(startPosition + positionShift * (i + 1) + new Vector3(0f, selectionYShift, 0f));
            Debug.Log(cardPositions[i]);
        }
        
    }

    private void SetCardsIntoPositions() {
        for (int i = 0; i < handCards.Count; i++) {
            handCards[i].transform.position = cardPositions[i];
        }
    }

    private void UpdateCardPositions() {
        CreateCardPositions();
        SetCardsIntoPositions();
    }
}
