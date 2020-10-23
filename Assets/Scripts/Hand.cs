using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Hand : MonoBehaviour
{
    public List<GameObject> handCards;
    // Start is called before the first frame update
    void Start()
    {
        handCards = new List<GameObject>();
        transform.position = new Vector3(0, -4.5f, 0);
    }


    //Add the newCard card to the hand
    public void AddCardToHand(GameObject newCard)
    {
        handCards.Add(newCard);
        newCard.GetComponent<Card>().SetCardState(CardState.Hand);
        AssembleCards();

    }

    //Relocate all cards when we add a card to the hand
    public void AssembleCards()
    {
        int i = 0;
        foreach(GameObject c in handCards)
        {
            c.GetComponent<Card>().MoveInHand(i, handCards.Count);
            i++;
        }
    }
}
