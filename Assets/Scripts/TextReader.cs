using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//Reads text from a file in resource folder. 
public class TextReader : MonoBehaviour {
    public string filename;
    string toSpeak;
    TextToSpeechManager tts;
    // Use this for initialization
    void Start () {
        //get text from chosen file
        var fileAsset = (TextAsset)Resources.Load(filename);
        toSpeak = fileAsset.text;   
    }
    public void onPause()
    {
        if (tts.IsSpeaking())
        {
            var audio = tts.AudioSource;
            audio.Pause();
        }
    }
    public void onContinue()
    {
        if (!tts.IsSpeaking())
        {
            var audio = tts.AudioSource;
            audio.Play();
        }
    }
    public void OnSpeak()
    {
        GazeManager gm = GazeManager.Instance;
        if (gm.IsGazingAtObject)
        {
            // Get the target object
            GameObject obj = gm.HitInfo.collider.gameObject;

            // Try and get a TTS Manager
            tts = null;
            if (obj != null)
            {
                tts = obj.GetComponent<TextToSpeechManager>();
                obj.GetComponent<TextToSpeechManager>().type = "Jokes";
            }
            //  Debug.Log("before");
            // If we have a text to speech manager on the target object, say something.
            // This voice will appear to emanate from the object.
            if (tts != null && !tts.IsSpeaking())
            {
                // Speak message. Can be text or SSML with correct command
                //tts.SpeakText(toSpeak);
                 tts.SpeakSsml(toSpeak);
            }
            else if (tts.IsSpeaking())
            {
                tts.StopSpeaking();
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
