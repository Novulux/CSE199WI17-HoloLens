/* Button that makes the message show the previous item in the current list */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class prevbutton : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject prevbtn;
    public GameObject text;

    // Use this for initialization
    void Start()
    {
        //Be careful of heirachy in Unity!
        prevbtn = transform.GetChild(1).gameObject;
        prevbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Triggers change to next item in list
    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        text.GetComponent<TextDisplay>().ChangePrev();
        Debug.Log("Previous Button Pressed");
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb) { }
}
