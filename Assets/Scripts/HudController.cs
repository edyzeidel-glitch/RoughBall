using UnityEngine;
using UnityEngine.UI;
using TMPro;

// On-screen HUD: hearts (top-left), LEVEL X (top-center), timer (top-right).
public class HudController : MonoBehaviour
{
    public Image[] hearts;
    public TMP_Text levelText;
    public TMP_Text timerText;
    public PlayerHealth player;

    void Start()
    {
        if (player != null)
        {
            player.OnHeartsChanged += UpdateHearts;
            UpdateHearts(player.hearts);
        }
    }

    void UpdateHearts(int h)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < h;
    }

    void Update()
    {
        var gm = GameManager.I;
        if (gm == null) return;
        levelText.text = "LEVEL " + gm.level;
        int t = (int)gm.elapsed;
        timerText.text = string.Format("{0:00}:{1:00}", t / 60, t % 60);
    }
}
