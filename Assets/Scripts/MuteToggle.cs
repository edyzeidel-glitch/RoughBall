using UnityEngine;
using TMPro;

// Toggles all game audio on/off. Sits on the start menu button.
public class MuteToggle : MonoBehaviour
{
    public TMP_Text label;

    private bool muted;

    public void Toggle()
    {
        muted = !muted;
        AudioListener.volume = muted ? 0f : 1f; // master volume for everything
        if (label != null) label.text = muted ? "SOUND: OFF" : "SOUND: ON";
    }
}
