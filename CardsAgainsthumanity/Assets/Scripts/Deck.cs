using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
    public List<GameObject> deckCards; 
    // Start is called before the first frame update
    void Start() {
        deckCards = GetDeckCards();
        ShuffleDeck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private List<GameObject> GetDeckCards() {
        List<GameObject> deckList = new List<GameObject>();
        foreach (Transform child in transform) {
            deckList.Add(child.gameObject);
        }
        return deckList;
    }

    public void ShuffleDeck() {
        List<GameObject> tmpList = new List<GameObject>();
        foreach (GameObject card in deckCards) {
            tmpList.Add(card);
        }

        List<int> placeList = GetRandomIndexList(deckCards.Count);
        deckCards.Clear();
        for (int i = 0; i < placeList.Count; i++) {
            deckCards.Add(tmpList[placeList[i]]);
        }

    }

    //shuffles a list of number from 0 to numberCount and returns the list of shuffled numbers
    private List<int> GetRandomIndexList(int numberCount) {
        List<int> numberList = new List<int>();
        for (int i = 0; i < numberCount; i++) {
            numberList.Add(i);
        }

        List<int> returnList = new List<int>();
        for (int i = 0; i < numberCount; i++) {
            int randomIndex = Random.Range(0, numberList.Count);
            returnList.Add(numberList[randomIndex]);
            numberList.RemoveAt(randomIndex);
        }

        return returnList;
    }
}
