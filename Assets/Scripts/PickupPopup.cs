using UnityEngine;
using TMPro;

// Shows a short announcement ("+1 HEART!", "IMMUNITY!") that pops and fades.
public class PickupPopup : MonoBehaviour
{
    public static PickupPopup I;
    public TMP_Text text;
    public float showTime = 1.5f;

    private float timer;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        if (text != null) text.alpha = 0f;
    }

    public void Show(string message, Color color)
    {
        text.text = message;
        text.color = color;
        timer = showTime;
    }

    void Update()
    {
        if (text == null) return;
        if (timer <= 0f)
        {
            if (text.alpha > 0f) text.alpha = 0f;
            return;
        }
        timer -= Time.deltaTime;
        float t = timer / showTime;
        // solid at first, fade in the last 40%
        text.alpha = Mathf.Clamp01(t / 0.4f);
        // slight pop at the start
        float s = 1f + 0.35f * Mathf.Clamp01((t - 0.85f) / 0.15f);
        text.transform.localScale = Vector3.one * s;
    }
}
