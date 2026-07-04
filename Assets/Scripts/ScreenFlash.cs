using UnityEngine;
using UnityEngine.UI;

// Full-screen color flash that fades out. Used for level-up (cyan) and damage (red).
public class ScreenFlash : MonoBehaviour
{
    public static ScreenFlash I;
    public Image img;

    private float timer, duration, maxAlpha;
    private Color color;

    void Awake()
    {
        I = this;
    }

    public void Flash(Color c, float alpha, float dur)
    {
        color = c;
        maxAlpha = alpha;
        timer = duration = dur;
    }

    void Update()
    {
        if (img == null) return;
        if (timer <= 0f)
        {
            if (img.color.a != 0f) img.color = new Color(0, 0, 0, 0);
            return;
        }
        timer -= Time.deltaTime;
        float a = maxAlpha * Mathf.Max(0f, timer / duration);
        img.color = new Color(color.r, color.g, color.b, a);
    }
}
