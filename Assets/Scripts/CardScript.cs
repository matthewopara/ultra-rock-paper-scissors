using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    [HideInInspector]
    public bool playAnimDone;
    public bool isCardBack;
    public DeckController deckController;
    public GameController gameController;
    public bool isHand;

    void Update()
    {
        if (isCardBack && deckController.deck.Count == 0)
        {
            if (isHand)
            {
                if (gameController.enemyCardLoc.ToString() == gameObject.tag && gameController.playerCard != null)
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void PlayAnimDone()
    {
        playAnimDone = true;
    }
}
