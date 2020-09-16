using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnimationScript : MonoBehaviour
{
    [HideInInspector]
    public bool winAnimDone = false;

    public ParticleSystem rockParticles;
    public ParticleSystem paperParticles;
    public ParticleSystem scissorsParticles;
    public ParticleSystem macheteParticles;
    public AudioSource rockAudio;
    public AudioSource paperAudio;
    public AudioSource scissorsAudio;
    public AudioSource dodgeAudio;
    public AudioSource macheteAudio;

    public AudioSource swingingAudio;
    public AudioSource scissorsOpenAudio;

    //0 = rock, 1 = paper, 2 = scissors, 3 = machete
    public int particleType;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(0f, 2f, 0f);

    }

    public void AnimDone()
    {
        winAnimDone = true;
    }

    public void PlayParticles()
    {
        if (particleType == 0)
        {
            rockParticles.gameObject.transform.position = transform.position;
            rockParticles.Play();
            rockAudio.Play();
}
        else if (particleType == 1)
        {
            paperParticles.gameObject.transform.position = transform.position;
            paperParticles.Play();
            paperAudio.Play();
        }
        else if (particleType == 2)
        {
            if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerScissors1"))
            {
                scissorsParticles.gameObject.transform.position = transform.position + offset;
                scissorsParticles.Play();
                scissorsAudio.Play();
            }
            else
            {
                scissorsParticles.gameObject.transform.position = transform.position - offset;
                scissorsParticles.Play();
                scissorsAudio.Play();
            }
        }
        else if (particleType == 3)
        {
            if (gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerMachete"))
            {
                macheteParticles.gameObject.transform.position = transform.position;
                if (macheteParticles.gameObject.transform.rotation == Quaternion.Euler(90f, 0f, 0f))
                {
                    macheteParticles.gameObject.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                }
                macheteParticles.Play();
                macheteAudio.Play();
            }
            else
            {
                macheteParticles.gameObject.transform.position = transform.position;
                if (macheteParticles.gameObject.transform.rotation == Quaternion.Euler(-90f, 0f, 0f))
                {
                    macheteParticles.gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                }
                macheteParticles.Play();
                macheteAudio.Play();
            }
        }
    }

    public void PlaySwingingSFX()
    {
        swingingAudio.Play();
    }

    public void PlayScissorsOpen()
    {
        scissorsOpenAudio.Play();
    }
}
