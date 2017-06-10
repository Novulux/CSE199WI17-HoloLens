/* Methods that make the head read all of your messages/lists */

using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechHead : MonoBehaviour {

    public GameObject text;
    public TextToSpeechManager textToSpeechManager;
    private bool stop;

    // Use this for initialization
    void Start () {
        stop = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator WaitTime(TextToSpeechManager tts)
    {
        Debug.Log("waiting");
        yield return new WaitUntil(() => tts.IsSpeaking() == true);

    }

    //Makes the head start reading whatever the current message is
    IEnumerator Play()
    {
        int t = text.GetComponent<TextDisplay>().Index();
        int i;
        for (i = t; i < text.GetComponent<TextDisplay>().ListSize(); i++)
        {
            if (stop)
            {
                stop = false;
                break;
            }
            Debug.Log(text.GetComponent<TextDisplay>().Text());
            textToSpeechManager.SpeakText(text.GetComponent<TextDisplay>().Text());
            yield return StartCoroutine(WaitTime(textToSpeechManager));
            yield return new WaitUntil(() => textToSpeechManager.IsSpeaking() == false);
            text.GetComponent<TextDisplay>().ChangeNext();
            Debug.Log("i is " + i + " and list size is " + text.GetComponent<TextDisplay>().ListSize());
        }
        //After it is done playing everything, user should be able to make the head read again
        GameObject.FindWithTag("HalfHeadTarget").GetComponent<speechbutton>().PlayTrue();
    }

    //Needed because can't call IEnurator methods in other scripts
    public void SpeechStart()
    {
        Debug.Log("IT CALLS SPEECHSTART");
        StartCoroutine(Play());
    }

    //Makes the head stop reading
    public void Stop()
    {
        stop = true;
        //If you stop halfway, you have to be able to read again
        GameObject.FindWithTag("HalfHeadTarget").GetComponent<speechbutton>().PlayTrue();
        textToSpeechManager.AudioSource.Stop();
    }
}
