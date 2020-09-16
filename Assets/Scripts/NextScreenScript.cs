using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextScreenScript : MonoBehaviour
{
    public RectTransform screens;
    private bool isClicked;
    private float timer;
    private Vector2 startPos;
    private Vector2 endPos;

    // Update is called once per frame
    void Update()
    {
        if (isClicked)
        {
            timer += Time.deltaTime * 1.5f;
        }
        else
        {
            timer = 0f;
        }
    }

    public void Right()
    {
        isClicked = true;
        StartCoroutine(MoveScreen(0));
    }

    public void Left()
    {
        isClicked = true;
        StartCoroutine(MoveScreen(1));
    }

    IEnumerator MoveScreen(int dir)
    {
        if (dir == 0)
        {
            startPos = screens.anchoredPosition;
            endPos = screens.anchoredPosition + new Vector2(-800f, 0f);

            while (screens.anchoredPosition.x != endPos.x)
            {
                screens.anchoredPosition = Vector3.Lerp(startPos, endPos, timer);
                yield return null;
            }
        }
        else if (dir == 1)
        {
            startPos = screens.anchoredPosition;
            endPos = screens.anchoredPosition + new Vector2(800f, 0f);

            while (screens.anchoredPosition.x != endPos.x)
            {
                screens.anchoredPosition = Vector3.Lerp(startPos, endPos, timer);
                yield return null;
            }
        }

        isClicked = false;
    }
}
