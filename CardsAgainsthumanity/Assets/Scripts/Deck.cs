using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
    public List<GameObject> deckCards; 
    // Start is called before the first frame update
    void Start() {
        deckCards = GetDeckCards();
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
}
