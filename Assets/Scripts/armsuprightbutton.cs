/* Controls the Stop function of both types of heads */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class armsuprightbutton : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject armsuprightbtn;
    public UnityEvent method;
    private GameObject head;

    // Use this for initialization
    void Start()
    {
        armsuprightbtn = transform.GetChild(1).gameObject;
        armsuprightbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
       head = armsuprightbtn.transform.parent.transform.FindChild("Head").gameObject;

        if(head == null)
        {
            Debug.Log("no head found");
            return;
        }
        //pause button on normal head or stop recording on other type
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
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {

    }
}
