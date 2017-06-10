using System.Collections;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This script controls the buttons for dictation. 
[RequireComponent(typeof(AudioSource), typeof(DictationManager))]
public class DictationStarter : MonoBehaviour {

    public Button RecordButton;
    public Button RecordStopButton;
    public Button PlayButton;
    public Button PlayStopButton;
    public Button TTS;

    public bool floating;

    public enum Message
    {
        PressMic,
        PressStop,
        SendMessage
    };
    public AudioSource dictationAudio;
    private DictationManager dictationManager;
    public bool called;
    // Use this for initialization
    void Start () {

        dictationManager = GetComponent<DictationManager>();
        PlayStopButton.enabled = false;
        floating = false;
        called = false;
    }
	
	// Update is called once per frame
	void Update () {
        // If the audio has stopped playing and the PlayStop button is still active,  reset the UI.
        if (!dictationAudio.isPlaying && PlayStopButton.enabled)
        {
            PlayStop();
        }
    }

    public void Record()
    {
         // Turn the microphone on, which returns the recorded audio.
         dictationAudio.clip = dictationManager.StartRecording();
        //only show the buttons if head is floating, else we can use the vuforia buttons.
        if (floating)
        {
            RecordButton.gameObject.SetActive(false);
            RecordStopButton.gameObject.SetActive(true);
        }
        called = true;
    }

    public void RecordStop()
    {
        //change button states if floating
        if (floating)
        {
            PlayButton.gameObject.SetActive(true);
            TTS.gameObject.SetActive(true);
            RecordButton.gameObject.SetActive(true);
            RecordStopButton.gameObject.SetActive(false);
        }
        // Restart the PhraseRecognitionSystem and KeywordRecognizer
        dictationManager.StopRecording();
        //  Debug.Log("going to restart speech system");
        // dictationManager.StartCoroutine("RestartSpeechSystem", GetComponent<KeywordManager>());

    }

    //message you left only needs to be played after making it float and placing in world
    public void Play()
    {
        PlayButton.gameObject.SetActive(false);
        PlayStopButton.gameObject.SetActive(true);

        dictationAudio.Play();
        PlayStopButton.enabled = true;
    }

    public void PlayStop()
    {
        PlayStopButton.gameObject.SetActive(false);
            
        PlayButton.gameObject.SetActive(true);
        PlayStopButton.enabled = false;
        dictationAudio.Stop();

    }

    void ResetAfterTimeout()
    {
        // Set proper UI state and play a sound.
       // SetUI(false, Message.PressMic, stopAudio);

        RecordStopButton.gameObject.SetActive(false);
        RecordButton.gameObject.SetActive(true);
    }
}
