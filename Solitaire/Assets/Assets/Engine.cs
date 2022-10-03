using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    // ace is a 1 
    // king is highest at 14 
    // if 1 or 4 card is black else card is red

    [SerializeField] private Sprite back_card;                      // back of card sprite.
    public List<Card> deck = new List<Card>();                      // deck of cards, only one in game.
    private Stack[] stack;                                          // the empty decks on the board
    private int que_deck = 52;                                      // number of cards in que.
    private int deck_iterator = 0;                                  // counts stuff
    [SerializeField] private List<Sprite> card_fronts;              // front sprite for cards
    public GameObject canvas;                                       // the games canvas 


    private void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("game");

        GameObject stack = GameObject.FindGameObjectWithTag("stack");

        this.stack = stack.GetComponentsInChildren<Stack>();

        generateDeck();

       // shuffle();

        setDecks();

    }

    private void generateDeck()         // function creates the card deck 
    {

        int k = 0;

        Card card = GameObject.FindGameObjectWithTag("deck").GetComponent<Card>();

        for (int i = 1; i < 14; i++)
        {
            for (int j = 1; j < 5; j++)
            {

                deck.Add(Instantiate(card).GetComponent<Card>());

                deck[k].transform.SetParent(canvas.transform);

                deck[k].transform.position = card.transform.position;

                deck[k].create_card(i, j, card_fronts[k], back_card);

                k++;
            }
        }

        Destroy(GameObject.FindGameObjectWithTag("deck"));

    }

    private void shuffle()
    {

    }

    // sets all the decks up 
    private void setDecks()
    {

        int k = 1;
        int l = 0;
        int down = 50;

        // 7 is the number of decks.
        // I is the deck I am acessing
        for (int i = 0; i < 7; i++)
        {

            // k is the number of cards
            for(int j = 0; j < k; j++)
            {

                deck[l].GetComponent<RectTransform>().anchoredPosition = stack[i].GetComponent<RectTransform>().anchoredPosition;

                

                deck[l].inStack = true;

                deck[l].stack = j;

                if(j > 0)
                {
                    var y = deck[l].GetComponent<RectTransform>().anchoredPosition.y;
                    var x = deck[l].GetComponent<RectTransform>().anchoredPosition.x;

                    deck[l].GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y - down);

                    down = down + 50;
                }

                if (j == k - 1)
                {
                    deck[l].flipCardFront();
                    deck[l].last_card = true;

                }

                l++;
            }

            down = 50;
            k++;

        }

        que_deck -= l;

    }

    public void move_deck()
    {

        deck_iterator++; 

        if(deck_iterator == que_deck)
        {
            for(int i = 0; i < deck.Count; i++)
            {
                if (!deck[i].inStack)
                {
                    deck[i].moveCardUp();
                }
            }
            deck_iterator = 0;
        }
    }

}