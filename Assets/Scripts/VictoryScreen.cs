using UnityEngine;
using TMPro;

// Victory panel: final score = time survived + level reached.
public class VictoryScreen : MonoBehaviour
{
    public static VictoryScreen I;

    public GameObject panel;
    public TMP_Text statsText;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        if (panel != null) panel.SetActive(false);
    }

    public void Show()
    {
        var gm = GameManager.I;
        int t = (int)gm.elapsed;
        statsText.text = string.Format("TIME {0:00}:{1:00}   LEVEL {2}", t / 60, t % 60, gm.level);
        panel.SetActive(true);
        if (AudioManager.I != null) AudioManager.I.PlayVictory();

    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
