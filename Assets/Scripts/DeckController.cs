using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    private ObjectPool objectPool;
    public List<GameObject> cardRefs;
    [HideInInspector]
    public List<GameObject> deck;
    //public List<GameObject> deckObj;

    [HideInInspector]
    public List<GameObject> hand;

    private GameObject temp;
    private int randomIndex;

    private bool canContinue;
    public GameController gameController;

    private Animator anim;
    private GameObject tempCard;

    [HideInInspector]
    public int emptyHandLoc = 0;
    private bool gameStart = true;

    private Camera cam;
    private Vector2 mousePos2D;

    [HideInInspector]
    public RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = ObjectPool.Instance;
        cam = Camera.main;
        deck = new List<GameObject>();
        hand = new List<GameObject>();

        //Fill the deck with cards
        for (int i = 0; i < 5; i++)
        {
            deck.Add(cardRefs[0]);
        }

        for (int i = 0; i < 5; i++)
        {
            deck.Add(cardRefs[1]);
        }

        for (int i = 0; i < 5; i++)
        {
            deck.Add(cardRefs[2]);
        }

        for (int i = 0; i < 3; i++)
        {
            deck.Add(cardRefs[3]);
        }

        for (int i = 0; i < 3; i++)
        {
            deck.Add(cardRefs[4]);
        }

        //Shuffle the deck
        for (int i = 0; i < deck.Count; i++)
        {
            temp = deck[i];
            randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        //Draw first 3 cards
        StartCoroutine("PauseThenDraw");
    }

    public void ChooseCard()
    {
        StartCoroutine("Wait");
    }

    IEnumerator Wait()
    {
        //wait until you click on a card
        while(true)
        {
            yield return null;
            yield return new WaitUntil (() => Input.GetMouseButtonDown(0));
            mousePos2D = cam.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.transform.gameObject.layer == 8 && (hit.transform.position.x == -3.75 || hit.transform.position.x == 0 || hit.transform.position.x == 3.75))
            {
                break;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (hit.transform.gameObject.layer == 8 && hit.transform.position.x == -3.75 + (i * 3.75))
            {
                emptyHandLoc = i;
            }
        }

        gameController.playerCard = hit.transform.gameObject;
        hand.RemoveAt(emptyHandLoc);
        hand.Insert(emptyHandLoc, cardRefs[5]);
        if (hand[0] == cardRefs[5] && hand[1] == cardRefs[5] && hand[2] == cardRefs[5])
        {
            hand.RemoveRange(0, 3);
        }
    }

    public void Draw(int num)
    {
        for (int i = 0; i < num; i++)
        {
            tempCard = objectPool.SpawnFromPool(deck[0].tag, transform.position, Quaternion.identity);
            anim = tempCard.GetComponent<Animator>();
            anim.SetTrigger("Draw-" + emptyHandLoc);
            hand.Remove(cardRefs[5]);
            hand.Insert(emptyHandLoc, deck[0]);
            deck.RemoveAt(0);

            if (gameStart)
            {
                emptyHandLoc += 1;
            }

            if (emptyHandLoc == 2)
            {
                gameStart = false;
            }
        }

        Debug.Log(hand[0].name);
        Debug.Log(hand[1].name);
        Debug.Log(hand[2].name);
    }

    IEnumerator PauseThenDraw()
    {
        yield return new WaitForSeconds(.8f);
        Draw(3);
    }
}
