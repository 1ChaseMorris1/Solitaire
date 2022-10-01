using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
{
    // +279.7 initial jump, after that the space is +189.3  
    // ace is a 1 
    // king is highest at 14 
    // if 1 or 4 card is black else card is red
    // for flipping card move -239 down

    [SerializeField] private Sprite back_card;      // back of card sprite 

    private static List<GameObject> deck = new List<GameObject>();                  // deck of cards
    private static List<GameObject> que_deck = new List<GameObject> ();            // que of flipped cards
    private static List<List<GameObject>> stacks = new List<List<GameObject>> ();   // stacks of cards on the board

    [SerializeField] private List<Sprite> card_fronts;               // front sprite for cards

    public static GameObject canvas; // the games canvas 

    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("game");

        generateDeck();

        shuffle();

        setDecks();

    }

    private void generateDeck()         // function creates the card deck 
    {
        int k = 0;
        GameObject Object = GameObject.FindGameObjectWithTag("deck");

        Color color = Object.GetComponent<Image>().color; 

        Object.GetComponent<Image>().color = Color.white;

        for (int i = 1; i < 14; i++)
        {
            for (int j = 1; j < 5; j++)
            {
                deck.Add(Instantiate(Object));

                deck[k].transform.SetParent(canvas.transform);

                deck[k].transform.position = Object.transform.position;

                deck[k].AddComponent<Card>().create_card(i, j, card_fronts[k], back_card,k);

                k++;
            }
        }

        Object.GetComponent<Image>().color = color;

    }

    private void shuffle()
    {
        List<Card> temp = new List<Card>(); 

        for(int i = 0; i < deck.Count; i++)
        {
            temp.Add(deck[i].GetComponent<Card>());
            Destroy(deck[i].GetComponent<Card>());
        }

        var count = temp.Count;
        var last = count - 1; 
        
        for(var i = 0; i < last; i++)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = temp[i]; temp[i] = temp[r]; temp[r] = tmp;
        }

        for (int i = 0; i < temp.Count; i++)
        {
            deck[i].AddComponent<Card>().equals(temp[i]);
        }

    }

    public static void move_card(int x)
    {
        if (deck.Count == que_deck.Count + 1)
        {
            for(int i = 0; i < deck.Count; i++)
                deck[i].GetComponent<Card>().moveCardUp();

            que_deck.Clear();
        }
        else
            que_deck.Add(deck[x]);
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
        int k = 0;
        // 7 is the number of decks.
        // I is the deck I am acessing
        for(int i = 0; i < 8; i++)
        {
            if (k == 0)
                jump = jump + 90.35F;
            else
                jump = jump + 189.3f;

            stacks.Add(new List<GameObject>());

            // k is the number of cards
            for(int j = 0; j < k; j++)
            {
                stacks[i].Add(deck[j]);

                stacks[i][j].GetComponent<RectTransform>().anchoredPosition = new Vector2(jump,269); 

                if (j > 0)
                {
                    var y = stacks[i][j].GetComponent<RectTransform>().anchoredPosition.y; 
                    var x = stacks[i][j].GetComponent<RectTransform>().anchoredPosition.x;

                    stacks[i][j].GetComponent<RectTransform>().anchoredPosition = new Vector2(y - 50,x);

                    stacks[i][j].GetComponent<Card>().flipCardFront();
                }

            }

            k++;

        }
    }

    // x is number of cards to set.
    // j is the distance to move the cards.
    // pos is position in deck. 
    private void setCards(int x, float j, int pos, GameObject card)
    {

    } 

}