using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Random;
enum Suit
{
    Clubs = 0,
    Diamonds = 1,
    Hearts = 2,
    Spades = 3
}
enum Rank
{
    A = 1,
    C2 = 2,
    C3 = 3,
    C4 = 4,
    C5 = 5,
    C6 = 6,
    C7 = 7,
    C8 = 8,
    C9 = 9,
    C10 = 10,
    J = 11,
    Q = 12,
    K = 13
}

public class Deck : MonoBehaviour
{
    float increment;
    public List<int> deck;
    [SerializeField] Sprite[] cardFaces;
    public GameObject cardPrefab;
    Hand hand;
    Coroutine dealingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        hand = FindObjectOfType<Hand>();
        increment = 0f;
        deck = GenerateDeck();
        Shuffle(deck);
    }

    // Update is called once per frame
    void Update()
    {
        HideDeckIfEmpty();
        
    }

    //Hide Deck sprite if it's empty
    private void HideDeckIfEmpty()
    {
        if (deck.Count == 0) { transform.position += new Vector3(0, 0, -20); };
    }
    //Deal() for the button Deal
    public void DealButton()
    {
        Deal();
    }
    //Deal() for the button Deal 8
    public void Deal8Button()
    {
        while(hand.handCards.Count < 8)
        {
            Deal();
        }
    }

    //Deal a card
    void Deal()
    {
        //create a card
        var newCard = Instantiate(cardPrefab, transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        var cCard = newCard.GetComponent<Card>();
        cCard.number = deck[0];
        cCard.oldspeed = 10f;
        cCard.hand = hand;
        cCard.SetSuitAndRank(deck[0]);
        newCard.GetComponent<SpriteRenderer>().sprite = cardFaces[deck[0]-1];
        hand.AddCardToHand(newCard);
        deck.RemoveAt(0);
        increment -= 0.01f;
    }

    //Generate Deck using with cards as numbers
    List<int> GenerateDeck()
    {
        List<int> newDeck = new List<int>();
        for (int i = 1; i < 53; i++)
        {
            newDeck.Add(i);
        }
        return newDeck;
    }
    /*Shuffle the deck with Fisher-Yates shuffle
    -- To shuffle an array a of n elements (indices 0..n-1):
     for i from n−1 downto 1 do
     j ← random integer such that 0 ≤ j ≤ i
     exchange a[j] and a[i]*/
    public void Shuffle(List<int> sDeck)
    {
        System.Random rng = new System.Random();
        for (int n = sDeck.Count - 1; n > 1; n--)
        {
            int j = rng.Next(n + 1);
            int val = sDeck[n];
            sDeck[n] = sDeck[j];
            sDeck[j] = val;
        }
    }

}