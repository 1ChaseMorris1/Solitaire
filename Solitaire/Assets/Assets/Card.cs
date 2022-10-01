using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler, IDragHandler
{

    private int type;           // spades, diamonds etc 
    private char color;         // black or red 
    public int Class;          // ace, two, king etc 
    private Sprite back;        // is used incase card is turned over
    private Sprite card;        // the front sprite of the card
    private Card nextCard;          // the next card in the stack 
    private int position;           // cards postition in the deck of cards
    private bool flipped;           // is card facing front

    public void create_card (int Class, int type, Sprite card, Sprite back, int p)
    {

        this.Class = Class;
        this.type = type;
        this.back = back;
        this.card = card;
        position = p;
        flipped = false;
        nextCard = null;

        if (type > 2)
            color = 'B';
        else
            color = 'R';

        gameObject.GetComponent<Image>().sprite = back;

    }

    public void equals(Card card)
    {
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

    public Card getNextCard()
    {
        return nextCard;
    }

    // moves the card down and flips it over. 
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!flipped)
        {
            var y = gameObject.GetComponent<RectTransform>().anchoredPosition.y;
            y = y - 239;
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-849f, y);
            flipCardFront();
            transform.SetSiblingIndex(55);
            Engine.move_card(position);
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
        }

    }

}
