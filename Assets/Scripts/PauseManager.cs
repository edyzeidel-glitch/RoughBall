using UnityEngine;

// P or Esc pauses/resumes the run. Every system already freezes
// when the game state isn't Playing, so pausing = flipping the state.
public class PauseManager : MonoBehaviour
{
    public GameObject pausedText;

    void Update()
    {
        var gm = GameManager.I;
        if (gm == null) return;

        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb == null) return;
        bool pressed = kb.pKey.wasPressedThisFrame || kb.escapeKey.wasPressedThisFrame;
        if (!pressed) return;

        if (gm.state == GameManager.State.Playing)
        {
            gm.state = GameManager.State.Paused;
            if (pausedText != null) pausedText.SetActive(true);
        }
        else if (gm.state == GameManager.State.Paused)
        {
            gm.state = GameManager.State.Playing;
            if (pausedText != null) pausedText.SetActive(false);
        }
    }
}
