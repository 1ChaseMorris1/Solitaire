using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IBeginDragHandler, IPointerUpHandler
{
    public Engine engine;                 // game engine
    private int type;                     // spades, diamonds etc 
    private char color;                   // black or red 
    public int Class;                     // ace, two, king etc 
    private Sprite back;                  // is used incase card is turned over
    private Sprite card;                  // the front sprite of the card
    private bool flipped;                 // is card facing front
    public bool inStack;                  // if the card is in the board decks then this will be true.
    public bool last_card;                // if the card is the last in the stack then a card can be placed onto it
    public Vector2 previousPosition;      // the previous position of the card. 
    public int stack;                     // what stack this card is in. 
    public int position;                  // the position in the stack the card is in.
    public bool moveMany;                 // if you are moving more than one card this variable is true.

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
        moveMany = false;

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
        // check to see if the gameobjects position is outside the bounds of the playing area 
        // if it is then on drop just go back to original position, do this in
        // the ''pointer up ' function. else the drop
        // script on the background will check if the card is in an arbitrary position on the 
        // background else the statements given will work. 
        if (flipped)
        {

            gameObject.GetComponent<RectTransform>().anchoredPosition +=
                eventData.delta / engine.canvas.GetComponent<Canvas>().scaleFactor;

            if (inStack && !(engine.stack[stack].getSize() == position + 1))
            {
                moveMany = true;
                engine.stack[stack].moveCards(position + 1, gameObject, transform.GetSiblingIndex());
                
            }
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (last_card)
        {
            eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 50);
            engine.stack[stack].addCard(eventData.pointerDrag.GetComponent<Card>());
        }
        else
        {
            if (inStack)
            {
                var x = eventData.pointerDrag.GetComponent<Card>().stack;
                var y = eventData.pointerDrag.GetComponent<Card>().position;
                engine.stack[x].moveCardsBack(y);
            } else
            {
                eventData.pointerDrag.GetComponent<Card>().revertPosition();
            }
        }

        moveMany = false;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(inStack)
        {
            engine.stack[stack].setPreviousPositions(position);

        } else
        {
            previousPosition = GetComponent<RectTransform>().anchoredPosition; 
        }

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        transform.SetSiblingIndex(60);
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
        }
        
    }

    // if the card is in the decks then it will do the movebackcards function 
    // else it will just revert to the single position because you can 
    // only pull one card out of the deck at a time. 
    public void OnPointerUp(PointerEventData eventData)
    {
        var x = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition.x;
        var y = eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition.y;


        if (x < -890 || x > 665)
        {
            gameObject.GetComponent<Card>().revertPosition();
            if (inStack)
                moveBackCards();
            else
                revertPosition();
        }

        if (y > 280 || y < -435)
        {
            gameObject.GetComponent<Card>().revertPosition();
            if (inStack)
                moveBackCards();
            else
                revertPosition();
        }

        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void moveBackCards()
    {
        engine.stack[stack].moveCardsBack(position);
    }

}
