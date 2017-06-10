/* This class controls the TextMesh that shows
 * the name of each list type. Ideally should be 
 * in sync with the other TextMesh.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListDisplay : MonoBehaviour {

    private string q;
    private string[] lists = new string[5];
    private int index;

	// Use this for initialization
	void Start () {
		q = "";
        index = -1;
        lists[0] = "ToDo List";
        lists[1] = "Passwords";
        lists[2] = "Messages";
        lists[3] = "Friends List";
        lists[4] = "Motivational Quotes";
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<TextMesh>().text = q;
    }

    //Displays the next name in the list
    public void ListNameNext()
    {
        index++;
        //ensures that the list loops around
        if(index > (lists.Length - 1))
        {
            index = 0;
        }
        q = lists[index];
    }

    //Displays the previous name in the list
    public void ListNamePrev()
    {
        index--;
        //ensures the list loops around
        if(index < 0)
        {
            index = (lists.Length) - 1;
        }
        q = lists[index];
    }
}
