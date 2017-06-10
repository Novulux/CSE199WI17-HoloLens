using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

namespace HoloToolkit.Unity.Tests
{
    public class TextToSpeechOnTap : MonoBehaviour, IInputClickHandler
    {
        public TextToSpeechManager TextToSpeech;
        private void Start()
        {

        }
        public void OnInputClicked(InputEventData eventData)
        {
            // If we have a text to speech manager on the target object, say something.
            // This voice will appear to emanate from the object.
          //  Debug.Log("beforeif");
            if (TextToSpeech != null)
            {
                // Get the name
                var voiceName = Enum.GetName(typeof(TextToSpeechVoice), TextToSpeech.Voice);

                // Create message
                var msg = string.Format("This is the {0} voice. ", voiceName);

                // Speak message
                TextToSpeech.SpeakText(msg);
            }
        }
    }
}
