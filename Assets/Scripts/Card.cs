//using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Experimental.UIElements;

public enum CardState { Hand, Dragged, Played};

public class Card : MonoBehaviour
{
    public Hand hand; 
    [SerializeField] float Rspeed = 10f; //rotating speed
    [SerializeField] Suit suit; //suit
    [SerializeField] Rank rank; //rank
    [SerializeField] CardState cardState;
    public int number; //number = suit*13 + rank
    Vector3 moveTo; //Where card should move to
    Vector3 rotateTo; //How card should rotate
    public float oldspeed;
    Vector3 oldRotateTo;
    bool m = false;

    // Start is called before the first frame update
    //Set card's state
    public void SetCardState(CardState c)
    {
        cardState = c;
    }

    //Set card's suit and rank
    public void SetSuitAndRank(int n)
    {
        suit = (Suit)(number / 13);
        rank = (Rank)(number % 13);
    }

    // Update is called once per frame
    void Update()
    {
        if (cardState != CardState.Played)
        {
            Move();
            Rotate();
           // Mouse();
            if (m) { Drag(); }
        }

    }
    /*
    private void Mouse()
    {

        if (EventSystem.OnMouseDrag)
        {
            print("YES");
        }
        //print(hit.collider.transform.gameObject.name);
    }
    */
    void OnMouseEnter()
    {
        if (cardState == CardState.Hand)
        {
            moveTo += new Vector3(0, 0, -5);
            oldRotateTo = rotateTo;
            rotateTo = new Vector3(0, 0, 0);
            oldspeed = 1000f;
            GetComponent<SpriteRenderer>().size = GetComponent<SpriteRenderer>().size*1.2f;
            //transform.localScale = new Vector3(0.4f, 0.4f, 0);
        }
        m = true;

    }
    void OnMouseExit()
    {
        if (cardState == CardState.Hand)
        {
            moveTo += new Vector3(0, 0, 5);
            rotateTo = oldRotateTo;
            oldspeed = 1000f;
            //transform.localScale = new Vector3(0.3f, 0.3f, 0);
            GetComponent<SpriteRenderer>().size = GetComponent<SpriteRenderer>().size / 1.2f;
            m = false;
        }

    }


    //Drag and drop
    private void Drag()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            cardState = CardState.Dragged;
            hand.handCards.Remove(gameObject);
            hand.AssembleCards();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            CheckPosition();
        }

    }



    //Check if the card is within the table range, if not, return it back to the hand
    private void CheckPosition()
    {
        if (transform.position.x > -6 && transform.position.x < 6  && transform.position.y > -3 && transform.position.y < 3)
        {
            cardState = CardState.Played;
            
        }
        else
        {
            hand.AddCardToHand(gameObject);
            moveTo += new Vector3(0, 0, -5);
            oldspeed = 20f;
        }
    }



    //Rescale and rotate the card when cursor enters the collider of the card in hand
    

    //Rescale and rotate the card back when cursor leaves the collider of the card in hand

    
    //Rotate the card in intended rotation Rotateto
    private void Rotate()
    {
        if (cardState == CardState.Hand)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotateTo), Time.deltaTime * Rspeed);
        }
        if (cardState == CardState.Dragged)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * 20f);
        }
    }

    //Move the card in its intended rotation moveTo
    private void Move()
    {
        
        if (cardState == CardState.Hand)
        {
            var movementThisFrame = oldspeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, moveTo, movementThisFrame);
            if (transform.position == moveTo) { oldspeed = 1f; }
        }
        if (cardState == CardState.Dragged)
        {
            var movementThisFrame = 50f * Time.deltaTime;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = transform.position.z;
            transform.position = Vector2.MoveTowards(transform.position, Camera.main.ScreenToWorldPoint(mousePos), movementThisFrame);
        }
    }

    //Move card in position pos in the hand with count cards and located in the handPos position
    public void MoveInHand(int pos, int count)
    {
        
 
        float angle = Mathf.PI/2 + (count / 3 - pos) * (Mathf.PI/24);
        float newX = 0f + 4f*Mathf.Cos(angle);
        float newY = -5.5f + 1f * Mathf.Sin(angle); ;
        moveTo = new Vector3(newX, newY, -1 - 0.001f * pos);
        rotateTo = new Vector3(0, 0, -90 + angle * 180 / Mathf.PI);
    }
}
