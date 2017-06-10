/* Plays and pauses the head user is looking at */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class wristright : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject wristrightbtn;
    public GameObject target;
    public Transform jonhead;

    // Use this for initialization
    void Start()
    {
        wristrightbtn = GameObject.Find("wristrightbtn");
        wristrightbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        GameObject head = target.transform.FindChild("Head").gameObject;

        if (head == null)
        {
            Debug.Log("no head found");
            return;
        }

        if (head.CompareTag("Head"))
        {
            SpeechCommands blah = new SpeechCommands();
            blah.buttonPause(head);
        }
        else if (head.CompareTag("MessageHead"))
        {
            Debug.Log("finds head");
            head.GetComponent<DictationStarter>().RecordStop();
        }
        Debug.Log("Right Wrist Button Pressed");
    }
    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {

    }
}
