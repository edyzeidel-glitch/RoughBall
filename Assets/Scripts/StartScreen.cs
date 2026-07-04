using UnityEngine;

// Start menu: title + how-to-play. START begins the run.
public class StartScreen : MonoBehaviour
{
    public GameObject panel;

    void Start()
    {
        if (panel != null)
            panel.SetActive(GameManager.I != null && GameManager.I.state == GameManager.State.Menu);
    }

    public void StartGame()
    {
        GameManager.I.state = GameManager.State.Playing;
        panel.SetActive(false);
    }
}
