using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource jewelVoice, blastVoice, finishVoice;

    private void Awake()
    {
        instance = this;
    }

    public void jewelVoiceEff()
    {
        jewelVoice.Stop();

        jewelVoice.pitch = Random.Range(0.8f, 1.2f);

        jewelVoice.Play();

    }

    public void blastVoiceEff()
    {
        blastVoice.Stop();

        blastVoice.pitch = Random.Range(0.8f, 1.2f);

        blastVoice.Play();

    }

    public void finishVoiceEff()
    {

        finishVoice.Play();

    }
}
