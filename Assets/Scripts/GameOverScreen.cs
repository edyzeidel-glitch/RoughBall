using UnityEngine;
using TMPro;

// Shows the Game Over panel when the player dies. Restart reloads the scene.
public class GameOverScreen : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text statsText;
    public PlayerHealth player;

    void Start()
    {
        if (player != null) player.OnDied += Show;
        if (panel != null) panel.SetActive(false);
    }

    void Show()
    {
        var gm = GameManager.I;
        int t = (int)gm.elapsed;
        statsText.text = string.Format("SURVIVED {0:00}:{1:00}   LEVEL {2}", t / 60, t % 60, gm.level);
        panel.SetActive(true);
        if (AudioManager.I != null) AudioManager.I.PlayGameOver();

    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
