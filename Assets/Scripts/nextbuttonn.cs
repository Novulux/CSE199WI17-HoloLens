/* This button displays the next message on the palm */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class nextbuttonn : MonoBehaviour, IVirtualButtonEventHandler
{

    private GameObject nextbtn;
    public GameObject text;

    // Use this for initialization
    void Start()
    {
        /* 
         * I had to use GetChild() because for some reason after a certain point,
         * the find method stopped working. So that means this actually depends on 
         * the heirarchy of each object.
         */
        nextbtn = transform.GetChild(0).gameObject;
        nextbtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        text.GetComponent<TextDisplay>().ChangeNext();
        Debug.Log("Next Button Pressed");
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb) {}
}
