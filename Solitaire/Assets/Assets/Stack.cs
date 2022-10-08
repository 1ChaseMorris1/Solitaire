using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Stack : MonoBehaviour, IDropHandler
{
    // add a remove card function to remove the cards from piles they have been moved from. 
    private List<Card> deck = new List<Card>();
    private Engine engine;

    private void Awake()
    {
    //    engine = GameObject.FindGameObjectWithTag("game").GetComponent<Engine>();
    }

    public void addCard(Card card)
    {
        // change the position of the card
        if(deck.Count > 0)
            deck[deck.Count - 1].GetComponent<Card>().last_card = false; 
        
        deck.Add(card);

        deck[deck.Count - 1].GetComponent<Card>().last_card = true;
    }

    public int getSize()
    {
        return deck.Count;
    }


    // obj is the moving card.
    public void moveCards(int x, GameObject obj, int sibling)
    {
        int down = 50; 
        for(int i = x; i < deck.Count; i++)
        {
            sibling++;
            deck[i].GetComponent<RectTransform>().anchoredPosition = obj.GetComponent<RectTransform>().anchoredPosition;
            deck[i].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, down);
            deck[i].gameObject.transform.SetSiblingIndex(sibling);
            down += 50;
        }
    }

    // moves all cards held back to original positions.
    public void moveCardsBack(int x)
    {
        for(int i = x; i < deck.Count; i++)
        {
            deck[i].GetComponent<Card>().revertPosition();
        }
    }

    public void setPreviousPositions(int x)
    {
        for(int i = x; i < deck.Count; i++)
        {
            deck[i].GetComponent<Card>().previousPosition =
                deck[i].GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

    }


}
