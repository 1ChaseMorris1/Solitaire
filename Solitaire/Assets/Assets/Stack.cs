using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Stack : MonoBehaviour, IDropHandler
{
    // add a remove card function to remove the cards from piles they have been moved from. 
    public List<Card> deck = new List<Card>();
    public int stackSize;
    //private Engine engine;

    private void Awake()
    {
     //   engine = GameObject.FindGameObjectWithTag("game").GetComponent<Engine>();
    }

    public void addCard(Card card, int stack)
    {
        card.inStack = true;

        card.stack = stack;

        card.last_card = true;

        card.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

        card.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, deck.Count * 50);

        deck.Add(card);

        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].GetComponent<Card>().position = i;
            if(i != deck.Count - 1)
                deck[i].GetComponent<Card>().last_card = false;
        }

    }

    // removes cards from the list 
    public void removeCard(int x)
    {
        deck.RemoveAt(x);

        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].position = i;
            deck[i].last_card = false;
        }

        deck[deck.Count - 1].last_card = true;
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

    // sets the previous positions of all cards 
    public void setPreviousPositions(int x)
    {
        for(int i = x; i < deck.Count; i++)
        {
            deck[i].GetComponent<Card>().previousPosition =
                deck[i].GetComponent<RectTransform>().anchoredPosition;
            stackSize++;
        }
    }

    


    // movement functions:
    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.GetComponent<Card>().revertPosition();
        eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = true;

    }


}
