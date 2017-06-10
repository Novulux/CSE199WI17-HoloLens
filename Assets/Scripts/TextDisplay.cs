/*
 * Defines the messages that can pop up and has
 * all of the functionality of the message display.
 * The methods get used by the next and previous buttons.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDisplay : MonoBehaviour {
    private string q;

    //list is defined in start()
    //Make sure x,y are equal because I use Sqrt to get number of items
    private string[ , ] lists = new string[5,5];
    //index0 is index of the list
    private int index0;
    //index is index of item in list
    private int index;

	// Use this for initialization
	void Start () {
        q = "";
        index0 = -1;
        index = -1;
        //Todo List
        lists[0,0] = "Buy milk"; 
        lists[0,1] = "Call mom";
        lists[0,2] = "Do your homework!!";
        lists[0,3] = "Do laundry";
        lists[0,4] = "Find Bob";

        //Passwords
        lists[1,0] = "Yahoo: 123456";
        lists[1,1] = "Tumblr: qwerty";
        lists[1,2] = "Reddit: 111111";
        lists[1,3] = "Facebook: password";
        lists[1,4] = "Instagram: 123123";

        //Messages
        lists[2,0] = "Bro 3:22PM: Help me w/ hw pls"; 
        lists[2,1] = "Wife 6:53PM: you make dinner";
        lists[2,2] = "Mom 9:00PM: I love you...jk xD";
        lists[2,3] = "Sister 9:46PM: you're annoying";
        lists[2,4] = "Dad 11:11PM: Where you?";

        //Friends Lists
        lists[3, 0] = "Jared Pham";
        lists[3, 1] = "Bryant Pham";
        lists[3, 2] = "[No Longer Available]";
        lists[3, 3] = "Swagmaster2000";
        lists[3, 4] = "[Empty]";

        //Motivational Quotes
        lists[4, 0] = "JUST DO ITT!";
        lists[4, 1] = "All you need is love";
        lists[4, 2] = "Dream Big";
        lists[4, 3] = "You oool";
        lists[4, 4] = ":D";

    }
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<TextMesh>().text = q;
	}

    //incrememnts to next item in list
    public void ChangeNext()
    {
        if(index0 < 0)
        {
            return;
        }

        index++;
        //makes sure it loops back around after reading end of list
        if(index > (Mathf.Sqrt(lists.Length) - 1))
        {
            index = 0;
        }
        q = lists[index0, index];
    }

    //decrements to previous item in list
    public void ChangePrev()
    {
        if(index0 < 0)
        {
            return;
        }
        index--;
        //makes sure it loops back when reaching beginning of list
        if(index < 0)
        {
            index = (int)(Mathf.Sqrt(lists.Length) - 1);
        }
        q = lists[index0, index];
    }

    //increments to the next list
    public void ChangeListNext()
    {
        index0++;
        //makes sure it loops back around
        if(index0 > (Mathf.Sqrt(lists.Length) - 1))
        {
            index0 = 0;
        }
        index = 0;
        q = lists[index0, index];
    }

    //decrements to the previous list
    public void ChangeListPrev()
    {
        index0--;
        //makes sure it loops around
        if(index0 < 0)
        {
            index0 = (int)(Mathf.Sqrt(lists.Length) - 1);
        }
        index = 0;
        q = lists[index0, index];
    }

    //List size getter
    public int ListSize()
    {
        return (int)(Mathf.Sqrt(lists.Length));
    }

    //Index getter
    public int Index()
    {
        return index;
    }

    //TextMesh string value getter
    public string Text()
    {
        return q;
    }
}
