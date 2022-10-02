using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    // +279.7 initial jump, after that the space is +189.3  
    // ace is a 1 
    // king is highest at 14 
    // if 1 or 4 card is black else card is red
    // for flipping card move -239 down

    [SerializeField] private Sprite back_card;      // back of card sprite 

    public static List<Card> deck = new List<Card>();                  // deck of cards
    private static List<List<int>> stacks = new List<List<int>>();      // stakcs of cards.

    private static int que_deck = 52;                                               // number of cards in que.
    private static int deck_iterator = 0;                                           // counts stuff

    [SerializeField] private List<Sprite> card_fronts;               // front sprite for cards

    public static GameObject canvas;                            // the games canvas 
    public List<Drop> eventDrop = new List<Drop> ();        

    private void Start()
    {

        canvas = GameObject.FindGameObjectWithTag("game");

        generateDeck();

        shuffle();

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

                deck[k].create_card(i, j, card_fronts[k], back_card,k);

                k++;
            }
        }

        Destroy(GameObject.FindGameObjectWithTag("deck"));

    }

    private void shuffle()
    {

    }

    // sets the decks on the board 
    /*****************************
     * 279.7 is the first X jump
     * after that 189.3 is the rest of the 
     * jumps, will keep track of this 
     * with a variable jump. 
     * static Y value is 269.
     * static X value is -849
     *****************************/
    private void setDecks()
    {
        float jump = -849f;
        int k = 1;
        int l = 0;
        //int z = 0;
        int down = 50;
        Drop drop = GameObject.FindGameObjectWithTag("eventdrop").GetComponent<Drop>();
        // 7 is the number of decks.
        // I is the deck I am acessing
        for (int i = 0; i < 7; i++)
        {
            stacks.Add(new List<int>());

            if (k == 1)
                jump = jump + 279.7f;
            else
                jump = jump + 189.3f;

            // k is the number of cards
            for(int j = 0; j < k; j++)
            {

                deck[l].GetComponent<RectTransform>().anchoredPosition = new Vector2(jump, 269);

                deck[l].inStack = true;

                stacks[i].Add(l);

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

                    /*
                    eventDrop.Add(Instantiate(drop));
                    eventDrop[z].transform.SetParent(canvas.transform);
                    eventDrop[z].transform.SetSiblingIndex(1);
                    eventDrop[z].GetComponent<RectTransform>().anchoredPosition = deck[l].GetComponent<RectTransform>().anchoredPosition;

                    z++;
                    */
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