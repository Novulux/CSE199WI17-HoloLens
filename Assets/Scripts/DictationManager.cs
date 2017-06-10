using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
//Taken from Microsoft's Holograms 212 MicrophoneManager 
public class DictationManager : MonoBehaviour {
    [Tooltip("A text area for the recognizer to display the recognized strings.")]
    public Text DictationDisplay;

    private DictationRecognizer dictationRecognizer;

    // Use this string to cache the text currently displayed in the text box.
    private StringBuilder textSoFar;

    // Using an empty string specifies the default microphone. 
    private static string deviceName = string.Empty;
    private int samplingRate;
    private const int messageLength = 60;

    // Use this to reset the UI once the Microphone is done recording after it was started.
    private bool hasRecordingStarted;

    void Awake()
    {
        dictationRecognizer = new DictationRecognizer();

        // This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
        dictationRecognizer.DictationHypothesis += DictationRecognizer_DictationHypothesis;

        // This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
        dictationRecognizer.DictationResult += DictationRecognizer_DictationResult;

        // This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
        dictationRecognizer.DictationComplete += DictationRecognizer_DictationComplete;

        // This event is fired when an error occurs.
        dictationRecognizer.DictationError += DictationRecognizer_DictationError;

        // Query the maximum frequency of the default microphone. Use 'unused' to ignore the minimum frequency.
        int unused;
        Microphone.GetDeviceCaps(deviceName, out unused, out samplingRate);

        // Use this string to cache the text currently displayed in the text box.
        textSoFar = new StringBuilder();

        // Use this to reset the UI once the Microphone is done recording after it was started.
        hasRecordingStarted = false;
    }

    void Update()
    {
        // check if running
        if (hasRecordingStarted && !Microphone.IsRecording(deviceName) && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            // Reset the flag now that we're cleaning up the UI.
            hasRecordingStarted = false;


            //this not called correctly?
            //Debug.Log("Called sendMessage");
            SendMessage("RecordStop");
        }
    }

    /// <summary>
    /// Turns on the dictation recognizer and begins recording audio from the default microphone.
    /// </summary>
    /// <returns>The audio clip recorded from the microphone.</returns>
    public AudioClip StartRecording()
    {
        // Shutdown the PhraseRecognitionSystem. This controls the KeywordRecognizers
        PhraseRecognitionSystem.Shutdown();
        textSoFar = new StringBuilder();
        dictationRecognizer.Start();

        DictationDisplay.text = "Dictation is starting. Say something...";
        hasRecordingStarted = true;

        // Start recording from the microphone for # seconds.
        return Microphone.Start(deviceName, false, messageLength, samplingRate);
    }

    /// <summary>
    /// Ends the recording session.
    /// </summary>
    public void StopRecording()
    {

        hasRecordingStarted = false;
        //Check if dictationRecognizer.Status is Running and stop it if so
        if (dictationRecognizer.Status == SpeechSystemStatus.Running)
        {

            dictationRecognizer.Stop();
            //dictationRecognizer.Dispose();
        }

        Microphone.End(deviceName);
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {

        yield return new WaitUntil(() => dictationRecognizer.Status  == SpeechSystemStatus.Stopped);
        PhraseRecognitionSystem.Restart();
    }
    /// <summary>
    /// This event is fired while the user is talking. As the recognizer listens, it provides text of what it's heard so far.
    /// </summary>
    /// <param name="text">The currently hypothesized recognition.</param>
    private void DictationRecognizer_DictationHypothesis(string text)
    {
        // 3.a: Set DictationDisplay text to be textSoFar and new hypothesized text
        // We don't want to append to textSoFar yet, because the hypothesis may have changed on the next event
        DictationDisplay.text = textSoFar.ToString() + " " + text + "...";
    }

    /// <summary>
    /// This event is fired after the user pauses, typically at the end of a sentence. The full recognized string is returned here.
    /// </summary>
    /// <param name="text">The text that was heard by the recognizer.</param>
    /// <param name="confidence">A representation of how confident (rejected, low, medium, high) the recognizer is of this recognition.</param>
    private void DictationRecognizer_DictationResult(string text, ConfidenceLevel confidence)
    {
        //Append textSoFar with latest text
        textSoFar.Append(text + ". ");

        //Set DictationDisplay text to be textSoFar
        DictationDisplay.text = textSoFar.ToString();
        ///could use confidence to determine whether message should be updated
    }

    /// <summary>
    /// This event is fired when the recognizer stops, whether from Stop() being called, a timeout occurring, or some other error.
    /// Typically, this will simply return "Complete". In this case, we check to see if the recognizer timed out.
    /// </summary>
    /// <param name="cause">An enumerated reason for the session completing.</param>
    private void DictationRecognizer_DictationComplete(DictationCompletionCause cause)
    {
        // If Timeout occurs, the user has been silent for too long.
        // With dictation, the default timeout after a recognition is 20 seconds.
        // The default timeout with initial silence is 5 seconds.
        if (cause == DictationCompletionCause.TimeoutExceeded)
        {
            Microphone.End(deviceName);

            DictationDisplay.text = "Dictation has timed out. Please press the record button again.";
            SendMessage("ResetAfterTimeout");
            StartCoroutine(WaitTime());
        }
    }

    /// <summary>
    /// This event is fired when an error occurs.
    /// </summary>
    /// <param name="error">The string representation of the error reason.</param>
    /// <param name="hresult">The int representation of the hresult.</param>
    private void DictationRecognizer_DictationError(string error, int hresult)
    {
        
        DictationDisplay.text = error + "\nHRESULT: " + hresult;
    }

    private IEnumerator RestartSpeechSystem(KeywordManager keywordToStart)
    {

        while (dictationRecognizer != null && dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            yield return null;
        }

        keywordToStart.StartKeywordRecognizer();
    }
}
