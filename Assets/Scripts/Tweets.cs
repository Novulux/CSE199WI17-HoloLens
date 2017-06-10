using UnityEngine.Windows.Speech;
using HoloToolkit.Unity;
using System;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;
using UnityEngine;
using System.Collections;
//This script is used to read tweets from an XML feed, similar to NewsReader
public class Tweets : MonoBehaviour {
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
        rdr = new rssreader("http://twitrss.me/twitter_user_to_rss/?user=realDonaldTrump");
        pausePos = rdr.rowNews.item.Count-1;
        i= rdr.rowNews.item.Count-1;
    }
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
    public void onContinue()
    {
        if (paused)
        {
            StartCoroutine(WaitForRemainder());
        }

    }

    public void OnAngry() 
    {
        //if we already read all of them and method is called, start over
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
            
            if (obj != null)
            {
               // tts = obj.GetComponent<TextToSpeechManager>();
                textToSpeechManager = obj.GetComponent<TextToSpeechManager>();
                obj.GetComponent<TextToSpeechManager>().type = "Tweet";
            }
            //  Debug.Log("before");
            // If we have a text to speech manager on the target object, say something.
            // This voice will appear to emanate from the object.
            if (textToSpeechManager != null && !textToSpeechManager.IsSpeaking())
            {
                StartCoroutine(Speaking(textToSpeechManager));

            }
            else if (textToSpeechManager.IsSpeaking())
            {
                textToSpeechManager.StopSpeaking();
            }
        }
    }
    IEnumerator WaitTime(TextToSpeechManager tts)
    {

        Debug.Log("waiting");
        //   i++;
        yield return new WaitUntil(() => tts.IsSpeaking() == true);

    }
    //we use this because we want have one tweet be fully read before moving on. 
    IEnumerator WaitForRemainder()
    {
        textToSpeechManager.AudioSource.Play();
        yield return StartCoroutine(WaitTime(textToSpeechManager));
        yield return new WaitUntil(() => textToSpeechManager.IsSpeaking() == false);
        paused = false;
        i = pausePos;
        OnAngry();
    }

    IEnumerator Speaking(TextToSpeechManager tts)
    {
        //reads from oldest to newest
        for (; i > -1;)
        {
            if (!paused)
            {
                Debug.Log(i);
                tts.SpeakText(rdr.rowNews.item[i].title);
                Debug.Log(rdr.rowNews.item[i].title);
                yield return StartCoroutine(WaitTime(tts));
                yield return new WaitUntil(() => tts.IsSpeaking() == false);
                if (paused)
                {
                    i--;
                    pausePos = i;
                    break;
                }
                i--;
            }
            else {
                pausePos = i;
                break;
            }
        }
    }
}

