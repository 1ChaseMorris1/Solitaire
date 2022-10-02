using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler, IDropHandler, IBeginDragHandler
{
    private Engine engine;  // game engine
    private int type;           // spades, diamonds etc 
    private char color;         // black or red 
    public int Class;          // ace, two, king etc 
    private Sprite back;        // is used incase card is turned over
    private Sprite card;        // the front sprite of the card
    private int position;           // cards postition in the deck of cards
    private bool flipped;           // is card facing front
    public bool inStack;           // if the card is in the stack then this will be true.
    public bool last_card;            // if the card is the last in the stack then a card can be placed onto it
    private GameObject nextCard;       // the next card in the line

    public void create_card(int Class, int type, Sprite card, Sprite back, int p)
    {
        nextCard = null;
        last_card = false;
        engine = GameObject.FindGameObjectWithTag("game").GetComponent<Engine>();
        this.Class = Class;
        this.type = type;
        this.back = back;
        this.card = card;
        position = p;
        flipped = false;
        inStack = false;

        if (type > 2)
            color = 'B';
        else
            color = 'R';

        GetComponent<Image>().sprite = back;

    }

    public void equals(Card card)
    {
        engine = card.engine;
        Class = card.Class;
        type = card.type;
        back = card.back;
        this.card = card.card;
        position = card.position;

        if (type > 2)
            color = 'B';
        else
            color = 'R';

        gameObject.GetComponent<Image>().sprite = card.back;
    }

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
            transform.SetSiblingIndex(55);
            engine.move_deck();
        }
    }

    // moves the card up and flips it over when the deck has been fully shuffled.
    public void moveCardUp()
    {
        var y = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
        y = y + 239;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-849f, y);
        flipCardBack();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (flipped)
        {

            gameObject.GetComponent<RectTransform>().anchoredPosition +=
                eventData.delta / Engine.canvas.GetComponent<Canvas>().scaleFactor;

            transform.SetSiblingIndex(60);
            // fix
            moveAllCards(eventData.pointerDrag.gameObject, 50);
            
            //transform.SetSiblingIndex(81);

            /*
            for(int i = 0; i < engine.eventDrop.Count; i++)
            {
                engine.eventDrop[i].transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            }
            */
        }

    }

    // when the appropriate card is dropped onto this card, the card will be set to 
    // the position of this card minus 50 

    // finds the last card in the stack and sets its end variable to true.
    private void addCard(GameObject card, Card next)
    {
        if (card == null)
        {
            card.AddComponent<Card>();
            card.GetComponent<Card>().equals(next); 
            card.GetComponent<Image>().sprite = null;
            return;

        } else
        {
           addCard(card.GetComponent<Card>().nextCard, next);
        }
    }

    private void moveAllCards(Card card, int tick)
    {
        if (card == null)
        {
            print("false");
            return;
        } else
        {
            print("made");

            card.transform.SetSiblingIndex(60);

            var y = GetComponent<RectTransform>().anchoredPosition.y;
            var x = GetComponent<RectTransform>().anchoredPosition.x;

            card.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y - tick);

            moveAllCards(card.GetComponent<Card>(), tick + 50);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

        var x = GetComponent<RectTransform>().anchoredPosition.x;
        var y = GetComponent<RectTransform>().anchoredPosition.y;

        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y - 50);

        print(type);

        addCard(this, eventData.pointerDrag.gameObject.GetComponent<Card>());

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}
