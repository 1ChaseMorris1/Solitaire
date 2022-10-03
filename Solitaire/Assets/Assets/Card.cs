using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IBeginDragHandler, IPointerUpHandler
{
    private Engine engine;                // game engine
    private int type;                     // spades, diamonds etc 
    private char color;                   // black or red 
    public int Class;                     // ace, two, king etc 
    private Sprite back;                  // is used incase card is turned over
    private Sprite card;                  // the front sprite of the card
    private bool flipped;                 // is card facing front
    public bool inStack;                  // if the card is in the stack then this will be true.
    public bool last_card;                // if the card is the last in the stack then a card can be placed onto it
    private Vector2 previousPosition;     // the previous position of the card. 
    public int stack;                     // what stack this card is in. 

    // ** MAYBE ** 
    private Card nextCard;                // the next card in the line.
    private Card previousCard;            // the previous card in the line. 

    public void create_card(int Class, int type, Sprite card, Sprite back)
    {
        nextCard = null;
        last_card = false;
        engine = GameObject.FindGameObjectWithTag("game").GetComponent<Engine>();
        this.Class = Class;
        this.type = type;
        this.back = back;
        this.card = card;
        flipped = false;
        inStack = false;

        if (type > 2)
            color = 'B';
        else
            color = 'R';

        GetComponent<Image>().sprite = back;

    }

    /*
    public void equals(Card card)
    {
        engine = card.engine;
        Class = card.Class;
        type = card.type;
        back = card.back;
        this.card = card.card;

        if (type > 2)
            color = 'B';
        else
            color = 'R';

        gameObject.GetComponent<Image>().sprite = card.back;
    }
    */

    public void flipCardBack()
    {
        gameObject.GetComponent<Image>().sprite = back;
        flipped = false;
    }

    public void flipCardFront()
    {
        gameObject.GetComponent<Image>().sprite = card;
        flipped = true;
    }

    // moves the card up and flips it over when the deck has been fully shuffled.
    public void moveCardUp()
    {
        var y = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        y = y + 239;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-849f, y);
        flipCardBack();
    }

    public void revertPosition()
    {
        GetComponent<RectTransform>().anchoredPosition = previousPosition;
    }

    /***********************************************************************
     * MOVEMENT FUNCTIONS                                                  *
     ***********************************************************************/ 

    public void OnDrag(PointerEventData eventData)
    {
        if (flipped)
        {

            gameObject.GetComponent<RectTransform>().anchoredPosition +=
                eventData.delta / engine.canvas.GetComponent<Canvas>().scaleFactor;

            transform.SetSiblingIndex(60);
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (last_card)
        {
            eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

            var x = GetComponent<RectTransform>().anchoredPosition.x;
            var y = GetComponent<RectTransform>().anchoredPosition.y;

            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y - 50);

        }
        else
        {
            eventData.pointerDrag.GetComponent<Card>().revertPosition();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousPosition = GetComponent<RectTransform>().anchoredPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }


    // moves the card down and flips it over. 
    public void OnPointerClick(PointerEventData eventData)
    {
        // if flipped 
        
        if (!flipped && !inStack)
        {
            var y = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
            y = y - 239;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-849f, y);
            flipCardFront();
            transform.SetSiblingIndex(60);
            engine.move_deck();
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerClick.GetComponent<Card>().flipped)
        {
            eventData.pointerDrag.GetComponent<Card>().revertPosition();
            eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

    }
}
