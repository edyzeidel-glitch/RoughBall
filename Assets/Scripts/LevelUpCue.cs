using UnityEngine;
using System.Collections;

// On level up: cyan screen flash + LEVEL text scale pop.
public class LevelUpCue : MonoBehaviour
{
    public TMPro.TMP_Text levelText;

    void Start()
    {
        if (GameManager.I != null) GameManager.I.OnLevelUp += OnLevel;
    }

    void OnLevel(int lvl)
    {
        if (ScreenFlash.I != null)
            ScreenFlash.I.Flash(new Color(0f, 1f, 0.95f), 0.45f, 0.5f);
        if (AudioManager.I != null) AudioManager.I.PlayLevelUp();

        StopAllCoroutines();
        StartCoroutine(Pop());
    }

    IEnumerator Pop()
    {
        float dur = 0.6f, t = 0f;
        Transform tr = levelText.transform;
        while (t < dur)
        {
            t += Time.deltaTime;
            // quick punch out, ease back
            float s = 1f + 0.9f * Mathf.Sin(Mathf.Clamp01(t / dur) * Mathf.PI);
            tr.localScale = Vector3.one * s;
            yield return null;
        }
        tr.localScale = Vector3.one;
    }
}
