using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonScript : MonoBehaviour
{
    public GameObject[] particles;
    private int randNum;
    public RectTransform screens;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ActivateParticles");
    }

    public void ButtonClicked()
    {
        randNum = Random.Range(0, 3);
        particles[randNum].GetComponent<ParticleSystem>().Play();
    }

    IEnumerator ActivateParticles()
    {
        while (true)
        {
            if (screens.anchoredPosition.x == 0)
            {
                randNum = Random.Range(0, 3);
                particles[randNum].GetComponent<ParticleSystem>().Play();
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
