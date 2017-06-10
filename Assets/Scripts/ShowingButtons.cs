using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowingButtons : MonoBehaviour {

    public GameObject buttonToShow;
   // public DictationStarter dicStarter;
	// Use this for initialization
	void Start () {
       // dicStarter = GameObject.FindGameObjectWithTag("SpeechManager").GetComponent<DictationStarter>();
	}
	public void onMessage(GameObject head)
    {
        //change tag to represent different type of head
        head.tag = "MessageHead";
        //if called after head is already floating, show record button
        if (head.GetComponent<DictationStarter>().floating)
        {
            head.GetComponent<DictationStarter>().RecordButton.gameObject.SetActive(true);
        }

        //should only be done when floating but do that later
        // uhhh idk what I was doing here, sorry
        // dicStarter.dictationAudio = head.GetComponent<AudioSource>();
    }

    //sets the record button to be active in case it was made a message head before floating
    public void onFloat()
    {
        buttonToShow.SetActive(true);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
