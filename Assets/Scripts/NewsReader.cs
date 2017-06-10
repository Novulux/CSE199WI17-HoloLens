using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using UnityEngine;
using System.Collections;

// This script reads out headlines and descriptions from NYT RSS Feeds. Different 
// news sites seem to format differently though, unfortunately. 
public class NewsReader : MonoBehaviour {

    private string text;
    // Use this for initialization
    public TextToSpeechManager textToSpeechManager;
    rssreader rdr;
    int i;
    bool paused;
    int pausePos;
    // Use this for initialization
    void Start()
    {
        // Set up a GestureRecognizer to detect Select gestures.
        paused = false;
        rdr = new rssreader();
        pausePos = rdr.rowNews.item.Count - 1;
        i = rdr.rowNews.item.Count - 1;
    }

    //if pause button pressed or command called 
    public void onPause()
    {
        if (!paused)
        {
            if (textToSpeechManager != null && textToSpeechManager.IsSpeaking())
            {
                paused = true;
                textToSpeechManager.AudioSource.Pause();
            }
        }

    }

    //resume reading if continue is called
    public void onContinue()
    {
        if (paused)
        {
            StartCoroutine(WaitForRemainder());
        }
    }

    public void OnNews()
    {
        //if we already read all the articles in file, restart
        if (i < 0)
        {
            i = rdr.rowNews.item.Count - 1;
        }
        // Debug.Log("evenfurther");
        GazeManager gm = GazeManager.Instance;
        if (gm.IsGazingAtObject)
        {
            // Get the target object
            GameObject obj = gm.HitInfo.collider.gameObject;

            // Try and get a TTS Manager
            TextToSpeechManager tts = null;
            if (obj != null)
            {
                tts = obj.GetComponent<TextToSpeechManager>();
                textToSpeechManager = obj.GetComponent<TextToSpeechManager>();
                obj.GetComponent<TextToSpeechManager>().type = "News";
            }
            //  Debug.Log("before");
            // If we have a text to speech manager on the target object, say something.
            // This voice will appear to emanate from the object.
            if (tts != null && !tts.IsSpeaking())
            {
                StartCoroutine(Speaking(tts));
            }
            else if (tts.IsSpeaking())
            {
                tts.StopSpeaking();
            }
        }
    }

    IEnumerator WaitForRemainder()
    {
        textToSpeechManager.AudioSource.Play();
        yield return StartCoroutine(WaitTime(textToSpeechManager));
        yield return new WaitUntil(() => textToSpeechManager.IsSpeaking() == false);
        paused = false;
        i = pausePos;
        OnNews();
    }
    IEnumerator WaitTime(TextToSpeechManager tts)
    {
        /* print(Time.time);
         yield return new WaitForSecondsRealtime(amt);
         print(Time.time);*/

      //  Debug.Log("waiting");
        //   i++;
        
        yield return new WaitUntil(() => tts.IsSpeaking() == true);

    }
    IEnumerator Speaking(TextToSpeechManager tts)
    {
        for (; i > -1;)
        {
            if (!paused)
            {
                tts.SpeakText(rdr.rowNews.item[i].title);
                //doesn't start speaking right away, so wait until it starts before checking when it stops
                yield return StartCoroutine(WaitTime(tts));
                yield return new WaitUntil(() => tts.IsSpeaking() == false);
                //check if we have paused while this was reading and exit.
                if (paused)
                {
                    i--;
                    pausePos = i;
                    break;
                }
                tts.SpeakText(rdr.rowNews.item[i].media_description);
                yield return StartCoroutine(WaitTime(tts));
                yield return new WaitUntil(() => tts.IsSpeaking() == false);
                if (paused)
                {
                    i--;
                    pausePos = i;
                    break;
                }
                tts.SpeakText(rdr.rowNews.item[i].description);
                yield return StartCoroutine(WaitTime(tts));
                yield return new WaitUntil(() => tts.IsSpeaking() == false);
                i--;
            }
            else
            {
                pausePos = i;
                break;
            }
        }
    }
}
