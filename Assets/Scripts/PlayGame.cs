using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public Animator transAnim;
    public int nextScene;

    public void Play()
    {
        StartCoroutine("LoadScene");
    }

    IEnumerator LoadScene()
    {
        transAnim.SetTrigger("Begin");
        yield return new WaitForSeconds(.8f);
        SceneManager.LoadScene(nextScene);
    }
}
