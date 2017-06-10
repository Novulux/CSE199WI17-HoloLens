/* Controls the start and record of each type of head */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class wristleft : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject wristleftbtn;
    public GameObject target;
    private int other;
    public UnityEvent method;
    // Use this for initialization
    void Start()
    {
        wristleftbtn = GameObject.Find("wristleftbtn");
        wristleftbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        target.transform.GetChild(0).gameObject.SetActive(true);
        other = 0;
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
            blah.buttonContinue(head);
        }
        else if (head.CompareTag("MessageHead"))
        {
            head.GetComponent<DictationStarter>().Record();
        }
        Debug.Log("Left Wrist Button Pressed");
    }
    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {

    }
}
