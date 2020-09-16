using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeScript : MonoBehaviour
{
    public AudioSource dodgeAudio;

    public void PlayDodgeSound()
    {
        dodgeAudio.Play();
    }
}
