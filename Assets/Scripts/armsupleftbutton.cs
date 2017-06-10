/* Controls the start functions of both types of heads */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class armsupleftbutton : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject armsupleftbtn;
    public UnityEvent method;
    public UnityEvent method1;
    // Use this for initialization
    void Start()
    {
        armsupleftbtn = transform.GetChild(0).gameObject;
        armsupleftbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
       
        GameObject head= armsupleftbtn.transform.parent.FindChild("Head").gameObject;
        if (head == null)
        {
            Debug.Log("no head found");
            return;
        }
        //if it's a regular head it's a continue button else a record button
        if (head.CompareTag("Head")){
            SpeechCommands blah = new SpeechCommands();
            blah.buttonContinue(head);
        }
        else if (head.CompareTag("MessageHead"))
        {
            head.GetComponent<DictationStarter>().Record();
        }
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {

    }
}
