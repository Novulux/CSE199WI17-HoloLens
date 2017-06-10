/*
 * This button controls the playing and pausing of the head
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class speechbutton : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject speechbtn;
    private bool play;

    // Use this for initialization
    void Start()
    {
        play = true;
        speechbtn = transform.GetChild(1).gameObject;
        speechbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        //if statement so if you push same button before the previous one is done, it will do the opposite
        if (play)
        {
            //Starts the speech by calling start method in SpeechHead script
            GameObject.FindWithTag("HalfHead").GetComponent<SpeechHead>().SpeechStart();
            play = false;
        }
        else
        {
            //Stops speech by calling stop method in SpeechHead script
            GameObject.FindWithTag("HalfHead").GetComponent<SpeechHead>().Stop();
            play = true;
        }
        Debug.Log("Speech Button Pressed");
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb) {}

    //Sets play to true. Used by SpeechHead script
    public void PlayTrue()
    {
        play = true;
    }
}
