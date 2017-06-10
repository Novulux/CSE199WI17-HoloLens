using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR.WSA.Input;

public class TTSMessage : MonoBehaviour
{

    public TextToSpeechManager textToSpeechManager;
    public Text DictationDisplay;

    int i;
    // Use this for initialization
    void Start()
    {

    }
    //reads back the text that is on the dicstation display
    public void readBack()
    {
        // Debug.Log("evenfurther");


            if (textToSpeechManager != null && !textToSpeechManager.IsSpeaking())
            {
                //  Debug.Log("Item Title: " + rdr.rowNews.item[0].title);
                var msg = DictationDisplay.text;
                 textToSpeechManager.SpeakText(msg);
            }
            else if (textToSpeechManager.IsSpeaking())
            {
                textToSpeechManager.StopSpeaking();
            }
        }
    }




