using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script contains all the speech commands used in the application.
/// </summary>
public class SpeechCommands : MonoBehaviour {

    //private bool floating;
    Rigidbody rb;
    public Transform Head;
	// Use this for initialization
	void Start () {
        //  floating = true;    
    }
    public void OnReset()
    {
        //reloads this scene
        SceneManager.LoadScene("withheads");
        //maybe just use  SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); for current scene
    }
    // turns on gravity for object being gazed upon.
    public void OnHeavy()
    {
        if (GazeManager.Instance.HitObject != null)
        {
            rb = GazeManager.Instance.HitObject.GetComponent<Rigidbody>();
            //check if object has rb and if not check for parent's (in case of messages)
            if (!rb)
            {
                rb = GazeManager.Instance.HitObject.transform.parent.GetComponent<Rigidbody>();
            }
            //check if it has a rigidbody
            if (rb)
            {
                //turn on gravity
                rb.useGravity = true;
                //floating = false;
            }
        }
    }

    //Turns off the gravity of an object and detaches it from the target if attached
    public void OnFloat()
    {
        if (GazeManager.Instance.HitObject != null)
        {
            GameObject curObject = GazeManager.Instance.HitObject;
            if (!curObject.GetComponent<Orbiting>().floating)
            {
                // {
                if (curObject.transform.parent != null)
                {
                    Transform parent = curObject.transform.parent;
                    Vector3 v = curObject.transform.position;
                    var oldRotation = curObject.transform.rotation;
                    curObject.transform.parent = null;
                    
                    //Transform newHead= Instantiate(head, v, Quaternion.identity, parent);
                    //move old head a bit above previous position so new head can take its place
                    if ((rb = curObject.GetComponent<Rigidbody>()) != null)
                        rb.MovePosition(curObject.transform.position + new Vector3(0, 0.1f, 0));
                    Transform newHead = Instantiate(Head, v, oldRotation,parent);

                    newHead.localScale = new Vector3(0.6F, 0.6F, 0.6F);
                    newHead.position = v;
                }

                if ((rb = curObject.GetComponent<Rigidbody>()) != null)
                {
                //    rb.MovePosition(curObject.transform.position + new Vector3(0, 0.05f, 0));
                    rb.useGravity = false;
                    curObject.GetComponent<Orbiting>().floating = true;
                }

                //if it is detached from targets/floating we can now move it around
                if ((curObject.GetComponent<HandDraggable>()) != null)
                {
                    (curObject.GetComponent<HandDraggable>()).enabled = true;
                }

                //if it is a head for dictating messages, we want to have certain buttons show up upon detaching
                if (curObject.CompareTag("MessageHead"))
                {
                    foreach (Transform child in curObject.transform)
                    {
                        if (child.gameObject.tag == "DictationText")
                        {
                            //child.gameObject.GetComponent<ShowingButtons>().onFloat();
                            curObject.GetComponent<DictationStarter>().RecordButton.gameObject.SetActive(true);
                            if (curObject.GetComponent<DictationStarter>().called)
                            {
                                curObject.GetComponent<DictationStarter>().PlayButton.gameObject.SetActive(true);
                                curObject.GetComponent<DictationStarter>().TTS.gameObject.SetActive(true);
                            }
                            curObject.GetComponent<DictationStarter>().floating = true;
                            break;
                        }
                    }
                }
                curObject.GetComponent<DictationStarter>().floating = true;
            }
        }
    }

    //this is called when head is changed to a message head
    public void onRecord()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        if (inFocus != null && inFocus.CompareTag("Head"))
        {
            foreach (Transform child in inFocus.transform)
            {
                if (child.gameObject.tag == "DictationText")
                {
                    if(child.gameObject.activeSelf == false)
                    {
                        Debug.Log("entered before assigning new tag");
                        Debug.Log("Tag is: " + inFocus.tag);
                        //show the dictation box and turn button on if necessary
                        child.gameObject.SetActive(true);
                        child.gameObject.GetComponent<ShowingButtons>().onMessage(inFocus);
                        Debug.Log("Tag is: " + inFocus.tag);
                    }
                }
            }
        }

    }

    //makes an object orbit the main camera
    public void onOrbit()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        Debug.Log("enters");
        if (inFocus != null && inFocus.GetComponent<Orbiting>() != null)
        {
            if (!inFocus.GetComponent<Orbiting>().getOrbit())
            {
                Debug.Log("calls orbit");
                inFocus.GetComponent<Orbiting>().Orbit();
            }
        }
    }

    //stops the gazed upon object from orbiting. Could also use gestures to drag from orbit. 
    public void StopOrbit()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        if (inFocus != null && inFocus.GetComponent<Orbiting>() != null)
        {
            if (inFocus.GetComponent<Orbiting>().getOrbit())
            {
                inFocus.GetComponent<Orbiting>().setOrbit(false);
            }
        }
    }

    //not used currently
    //if we wanted to only assign tweets to a specific head it could be used
    public void OnAngry()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        if (inFocus != null && inFocus.GetComponent<Tweets>() != null)
        {
            inFocus.GetComponent<Tweets>().OnAngry();
        }
    }

    //pauses the head according to its SpeechManager type. 
    public void OnPause()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        if (inFocus != null && inFocus.GetComponent<TextToSpeechManager>() != null)
        {
            var SpeechManager = GameObject.FindWithTag("SpeechManager");
            if (inFocus.GetComponent<TextToSpeechManager>().type == "Tweet")
            {
                SpeechManager.GetComponent<Tweets>().onPause();
            }
            else if(inFocus.GetComponent<TextToSpeechManager>().type == "Jokes")
            {
                SpeechManager.GetComponent<TextReader>().onPause();
            }
            else if (inFocus.GetComponent<TextToSpeechManager>().type == "News")
            {
                SpeechManager.GetComponent<NewsReader>().onPause();
            }
        }
    }
    //resumes the head according to its SpeechManager type.
    public void OnContinue()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        if (inFocus != null && inFocus.GetComponent<TextToSpeechManager>() != null)
        {
            var SpeechManager = GameObject.FindWithTag("SpeechManager");
            if (inFocus.GetComponent<TextToSpeechManager>().type == "Tweet")
            {
                SpeechManager.GetComponent<Tweets>().onContinue();
            }
            else if (inFocus.GetComponent<TextToSpeechManager>().type == "Jokes")
            {
                SpeechManager.GetComponent<TextReader>().onContinue();
            }
            else if (inFocus.GetComponent<TextToSpeechManager>().type == "News")
            {
                SpeechManager.GetComponent<NewsReader>().onContinue();
            }
        }
    }

    //Pauses all existing heads
    public void onQuiet()
    {
        GameObject[] heads = GameObject.FindGameObjectsWithTag("Head");
        //change to stop later maybe add variable to completely exit in tweets
        foreach(GameObject head in heads)
        {
            var SpeechManager = GameObject.FindWithTag("SpeechManager");
            if (head.GetComponent<TextToSpeechManager>().type == "Tweet")
            {
                SpeechManager.GetComponent<Tweets>().onPause();
            }
            else if (head.GetComponent<TextToSpeechManager>().type == "Jokes")
            {
                SpeechManager.GetComponent<TextReader>().onPause();
            }
            else if(head.GetComponent<TextToSpeechManager>().type == "News")
            {
                SpeechManager.GetComponent<NewsReader>().onPause();
            }
        }
    }

    //Destroys the head being gazed at
    public void Destroy()
    {
        GameObject inFocus = GazeManager.Instance.HitObject;
        if (inFocus != null)
        {
            if (inFocus.tag == "Head" || inFocus.tag == "MessageHead")
            {
                Destroy(inFocus);
            }
        }
    }

    //button version of OnContinue, not a voice command
    public void buttonContinue(GameObject head)
    {
        var SpeechManager = GameObject.FindWithTag("SpeechManager");
        if (head.GetComponent<TextToSpeechManager>().type == "Tweet")
        {
            SpeechManager.GetComponent<Tweets>().onContinue();
        }
        else if (head.GetComponent<TextToSpeechManager>().type == "Jokes")
        {
            SpeechManager.GetComponent<TextReader>().onContinue();
        }
        else if (head.GetComponent<TextToSpeechManager>().type == "News")
        {
            SpeechManager.GetComponent<NewsReader>().onContinue();
        }
    }

    //button version of OnPause, not a voice command
    public void buttonPause(GameObject head)
    {
        var SpeechManager = GameObject.FindWithTag("SpeechManager");
        if (head.GetComponent<TextToSpeechManager>().type == "Tweet")
        {
            SpeechManager.GetComponent<Tweets>().onPause();
        }
        else if (head.GetComponent<TextToSpeechManager>().type == "Jokes")
        {
            SpeechManager.GetComponent<TextReader>().onPause();
        }
        else if (head.GetComponent<TextToSpeechManager>().type == "News")
        {
            SpeechManager.GetComponent<NewsReader>().onPause();
        }
    }

    // Update is called once per frame
    void Update () {
		//add float effect maybe
	}
}
