using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarScript : MonoBehaviour
{
    public bool isPlayer;
    public Sprite[] spritePool;
    public SpriteRenderer spriteRenderer;
    public GameController gameController;
    public Text endText;


    // Update is called once per frame
    void Update()
    {
        if (gameController.playingAnim)
        {
            if (isPlayer)
            {
                PlayerReact();
            }
            else
            {
                EnemyReact();
            }
        }

        if (!gameController.playingAnim)
        {
            spriteRenderer.sprite = spritePool[2];
        }

        if (gameController.playerWon && isPlayer)
        {
            //gameObject.GetComponent<Animator>().SetTrigger("Play");
            gameObject.GetComponent<Animator>().enabled = true;
            endText.text = "You Win";
        }
        else if (gameController.enemyWon && !isPlayer)
        {
            //gameObject.GetComponent<Animator>().SetTrigger("Play");
            gameObject.GetComponent<Animator>().enabled = true;
            endText.text = "You Lose";
        }
        else if (gameController.tiedGame)
        {
            endText.text = "Tied Game";
        }
    }

    void PlayerReact()
    {
        if (gameController.roundOutcome == 0)
        {
            spriteRenderer.sprite = spritePool[0];
        }
        else if (gameController.roundOutcome == 1)
        {
            spriteRenderer.sprite = spritePool[1];
        }
        else
        {
            spriteRenderer.sprite = spritePool[2];
        }
    }

    void EnemyReact()
    {
        if (gameController.roundOutcome == 0)
        {
            spriteRenderer.sprite = spritePool[1];
        }
        else if (gameController.roundOutcome == 1)
        {
            spriteRenderer.sprite = spritePool[0];
        }
        else
        {
            spriteRenderer.sprite = spritePool[2];
        }
    }
}
