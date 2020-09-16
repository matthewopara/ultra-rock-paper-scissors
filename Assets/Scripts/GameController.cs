using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<GameObject> cardRefs;
    [HideInInspector]
    public List<GameObject> deck;
    private GameObject CPUCard;
    public DeckController deckController;
    private int randomNum;

    [HideInInspector]
    public GameObject playerCard;

    [HideInInspector]
    public int playerScore;

    [HideInInspector]
    public int CPUScore;

    private Animator anim;

    private bool playAnimDone;

    private CardScript cardScript;

    public GameObject[] cardBacks;

    public GameObject enemyCard;
    public SpriteRenderer enemyCardRenderer;
    public int enemyCardLoc;
    private List<int> finalHand;
    private int randInt;

    //0 = Win, 1 = Lost, 2 = Dodge, 3 = Same
    [HideInInspector]
    public int roundOutcome = 2;
    public Animator rockWinAnim;
    public Animator paperWinAnim;
    public Animator scissorsWinAnim1;
    public Animator scissorsWinAnim2;
    public Animator macheteWinAnim;
    public Animator dodgeWinAnim;

    private WinAnimationScript winAnimScript;

    public Text playerScoreTxt;
    public Text enemyScoreTxt;
    public Animator plusOneAnim;

    [HideInInspector]
    public bool playingAnim;

    [HideInInspector]
    public bool playerWon;

    [HideInInspector]
    public bool enemyWon;

    [HideInInspector]
    public bool tiedGame;

    public GameObject playAgainButton;
    
    // Start is called before the first frame update
    void Start()
    {
        deck = new List<GameObject>();
        finalHand = new List<int>();
        roundOutcome = 2;

        for(int i = 0; i < 3; i++)
        {
            finalHand.Add(i);
        }

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("finalHand[" + i + "] = " + finalHand[i]);
        }

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

        StartCoroutine("Pause");
        StartCoroutine("Game");
    }

    IEnumerator Game()
    {
        do {
            Debug.Log(deck.Count);
            randomNum = Random.Range(0, deck.Count - 1);
            if (deck.Count > 0)
            {
                CPUCard = deck[randomNum];
            }

            ChangeCardSprite(CPUCard.tag);
            if (deck.Count > 0)
            {
                deck.RemoveAt(randomNum);
            }
            deckController.ChooseCard();

            if (deck.Count > 3)
            {
                enemyCardLoc = Random.Range(0, 3);
                Debug.Log(enemyCardLoc);
            }
            else if (deck.Count == 3)
            {
                enemyCardLoc = Random.Range(0, 3);
                Debug.Log(enemyCardLoc);
            }
            else
            {
                randInt = Random.Range(0, finalHand.Count);
                Debug.Log("randInt = " + randInt);
                Debug.Log("finaHand Count = " + finalHand.Count);
                enemyCardLoc = finalHand[randInt];
                finalHand.Remove(finalHand[randInt]);
                Debug.Log(enemyCardLoc);
            }

            yield return new WaitUntil(() => playerCard != null);
            anim = deckController.hit.transform.gameObject.GetComponent<Animator>();
            anim.SetTrigger("Play-" + deckController.emptyHandLoc);
            anim = enemyCard.GetComponent<Animator>();
            anim.SetTrigger("Play-" + enemyCardLoc);
            cardBacks[enemyCardLoc].SetActive(false);
            cardScript = playerCard.GetComponent<CardScript>();
            yield return new WaitUntil(() => cardScript.playAnimDone);
            cardScript.playAnimDone = false;

            CompareCards(CPUCard, playerCard);
            PlayAnimation();
            playingAnim = true;
            yield return new WaitUntil(() => roundOutcome == 3 || winAnimScript.winAnimDone);
            playingAnim = false;
            if (roundOutcome != 3)
            {
                winAnimScript.winAnimDone = false;
            }
            anim = enemyCard.GetComponent<Animator>();
            anim.SetTrigger("Default");
            ChangeScores();

            if (deckController.deck.Count > 0)
            {
                deckController.Draw(1);
                cardBacks[enemyCardLoc].SetActive(true);
            }
            playerCard.SetActive(false);
            playerCard = null;
        } while (deckController.hand.Count > 0);

        if (playerScore > CPUScore)
        {
            playerWon = true;
        }
        else if (playerScore < CPUScore)
        {
            enemyWon = true;
        }
        else if (playerScore == CPUScore)
        {
            tiedGame = true;
        }

        playAgainButton.SetActive(true);
    }

    void ChangeScores()
    {
        if (CPUScore.ToString() != enemyScoreTxt.text)
        {
            enemyScoreTxt.text = CPUScore.ToString();
            plusOneAnim.SetTrigger("Enemy");
        }

        if (playerScore.ToString() != playerScoreTxt.text)
        {
            playerScoreTxt.text = playerScore.ToString();
            plusOneAnim.SetTrigger("Player");
        }
    }

    void CompareCards(GameObject cpu, GameObject player)
    {
        if (player.CompareTag("Rock"))
        {
            switch (cpu.tag)
            {
                case "Rock":
                    Debug.Log("Same");
                    roundOutcome = 3;
                    break;
                case "Paper":
                    Debug.Log("Lost");
                    CPUScore += 1;
                    roundOutcome = 1;
                    break;
                case "Scissors":
                    Debug.Log("Won");
                    playerScore += 1;
                    roundOutcome = 0;
                    break;
                case "Machete":
                    Debug.Log("Lost");
                    CPUScore += 1;
                    roundOutcome = 1;
                    break;
                case "Dodge":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
            }
        }

        if (player.CompareTag("Paper"))
        {
            switch (cpu.tag)
            {
                case "Paper":
                    Debug.Log("Same");
                    roundOutcome = 3;
                    break;
                case "Scissors":
                    Debug.Log("Lost");
                    CPUScore += 1;
                    roundOutcome = 1;
                    break;
                case "Rock":
                    Debug.Log("Won");
                    playerScore += 1;
                    roundOutcome = 0;
                    break;
                case "Machete":
                    Debug.Log("Lost");
                    CPUScore += 1;
                    roundOutcome = 1;
                    break;
                case "Dodge":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
            }
        }

        if (player.CompareTag("Scissors"))
        {
            switch (cpu.tag)
            {
                case "Scissors":
                    Debug.Log("Same");
                    roundOutcome = 3;
                    break;
                case "Rock":
                    Debug.Log("Lost");
                    CPUScore += 1;
                    roundOutcome = 1;
                    break;
                case "Paper":
                    Debug.Log("Won");
                    playerScore += 1;
                    roundOutcome = 0;
                    break;
                case "Machete":
                    Debug.Log("Lost");
                    CPUScore += 1;
                    roundOutcome = 1;
                    break;
                case "Dodge":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
            }
        }

        if (player.CompareTag("Machete"))
        {
            switch (cpu.tag)
            {
                case "Scissors":
                    Debug.Log("Won");
                    playerScore += 1;
                    roundOutcome = 0;
                    break;
                case "Rock":
                    Debug.Log("Won");
                    playerScore += 1;
                    roundOutcome = 0;
                    break;
                case "Paper":
                    Debug.Log("Won");
                    playerScore += 1;
                    roundOutcome = 0;
                    break;
                case "Machete":
                    Debug.Log("Same");
                    roundOutcome = 3;
                    break;
                case "Dodge":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
            }
        }

        if (player.CompareTag("Dodge"))
        {
            switch (cpu.tag)
            {
                case "Scissors":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
                case "Rock":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
                case "Paper":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
                case "Machete":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
                case "Dodge":
                    Debug.Log("Draw");
                    roundOutcome = 2;
                    break;
            }
        }

    }

    void ChangeCardSprite(string card)
    {
        enemyCard.tag = card;
        switch (card)
        {
            case "Rock":
                enemyCard.GetComponent<SpriteRenderer>().sprite = cardRefs[0].GetComponent<SpriteRenderer>().sprite;
                break;
            case "Paper":
                enemyCard.GetComponent<SpriteRenderer>().sprite = cardRefs[1].GetComponent<SpriteRenderer>().sprite;
                break;
            case "Scissors":
                enemyCard.GetComponent<SpriteRenderer>().sprite = cardRefs[2].GetComponent<SpriteRenderer>().sprite;
                break;
            case "Machete":
                enemyCard.GetComponent<SpriteRenderer>().sprite = cardRefs[3].GetComponent<SpriteRenderer>().sprite;
                break;
            case "Dodge":
                enemyCard.GetComponent<SpriteRenderer>().sprite = cardRefs[4].GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }

    void PlayAnimation()
    {
        if (roundOutcome == 0)
        {
            switch (playerCard.tag)
            {
                case "Rock":
                    rockWinAnim.SetTrigger("Player");
                    winAnimScript = rockWinAnim.gameObject.GetComponent<WinAnimationScript>();
                    break;
                case "Paper":
                    paperWinAnim.SetTrigger("Player");
                    winAnimScript = paperWinAnim.gameObject.GetComponent<WinAnimationScript>();
                    break;
                case "Scissors":
                    scissorsWinAnim1.SetTrigger("Player");
                    scissorsWinAnim2.SetTrigger("Player");
                    winAnimScript = scissorsWinAnim1.gameObject.GetComponent<WinAnimationScript>();
                    break;
                case "Machete":
                    macheteWinAnim.SetTrigger("Player");
                    winAnimScript = macheteWinAnim.gameObject.GetComponent<WinAnimationScript>();
                    break;
            }
        }
        else if (roundOutcome == 1)
        {
            switch (CPUCard.tag)
            {
                case "Rock":
                    rockWinAnim.SetTrigger("Enemy");
                    winAnimScript = rockWinAnim.gameObject.GetComponent<WinAnimationScript>();
                    break;
                case "Paper":
                    paperWinAnim.SetTrigger("Enemy");
                    winAnimScript = paperWinAnim.gameObject.GetComponent<WinAnimationScript>();
                    break;
                case "Scissors":
                    scissorsWinAnim1.SetTrigger("Enemy");
                    scissorsWinAnim2.SetTrigger("Enemy");
                    winAnimScript = scissorsWinAnim1.gameObject.GetComponent<WinAnimationScript>();
                    break;
                case "Machete":
                    macheteWinAnim.SetTrigger("Enemy");
                    winAnimScript = macheteWinAnim.gameObject.GetComponent<WinAnimationScript>();
                    break;
            }
        }
        else if (roundOutcome == 2)
        {
            dodgeWinAnim.SetTrigger("Dodge");
            winAnimScript = dodgeWinAnim.gameObject.GetComponent<WinAnimationScript>();
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(.8f);
        foreach (GameObject g in cardBacks)
        {
            g.SetActive(true);
        }
        //cardBack0.enabled = true;
        //cardBack1.enabled = true;
        //cardBack2.enabled = true;
    }
}
