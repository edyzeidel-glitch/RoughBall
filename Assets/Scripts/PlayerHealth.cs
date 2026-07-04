using UnityEngine;

// 3 hearts. Paddle hit = -1 heart + 1.5s invincibility with rapid red flash.
// 0 hearts = Game Over. Shield pickup hooks in later via SetShield().
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHearts = 3;
    public int hearts = 3;
    public float invincibleTime = 1.5f;

    public System.Action<int> OnHeartsChanged;
    public System.Action OnDied;

    private float invTimer;
    private Renderer rend;
    private Color baseEmission;
    private Color hurtEmission = new Color(3f, 0.05f, 0.05f); // HDR red
    private Color shieldEmission = new Color(2.5f, 1.8f, 0.2f); // HDR gold
    private float shieldTimer;

    public bool IsInvincible { get { return invTimer > 0f || shieldTimer > 0f; } }

    void Awake()
    {
        rend = GetComponent<Renderer>();
        baseEmission = rend.material.GetColor("_EmissionColor");
    }

    void Update()
    {
        if (shieldTimer > 0f)
        {
            shieldTimer -= Time.deltaTime;
            rend.material.SetColor("_EmissionColor", shieldEmission);
            if (shieldTimer <= 0f) rend.material.SetColor("_EmissionColor", baseEmission);
            return;
        }

        if (invTimer > 0f)
        {
            invTimer -= Time.deltaTime;
            // rapid red flash: toggle ~10 times per second
            bool flashOn = Mathf.FloorToInt(invTimer * 10f) % 2 == 0;
            rend.material.SetColor("_EmissionColor", flashOn ? hurtEmission : baseEmission);
            if (invTimer <= 0f) rend.material.SetColor("_EmissionColor", baseEmission);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Paddle")) TakeDamage();
    }

    public void TakeDamage()
    {
        if (IsInvincible) return;
        if (GameManager.I != null && GameManager.I.state != GameManager.State.Playing) return;

        hearts--;
        if (OnHeartsChanged != null) OnHeartsChanged(hearts);
        if (ScreenShake.I != null) ScreenShake.I.Shake(0.35f, 0.3f);
        if (ScreenFlash.I != null) ScreenFlash.I.Flash(new Color(1f, 0.1f, 0.1f), 0.35f, 0.3f);
        if (AudioManager.I != null) AudioManager.I.PlayHit();



        if (hearts <= 0)
        {
            GameManager.I.state = GameManager.State.GameOver;
            if (OnDied != null) OnDied();
        }
        else
        {
            invTimer = invincibleTime;
        }
    }

    public void Heal(int amount)
    {
        hearts = Mathf.Min(maxHearts, hearts + amount);
        if (OnHeartsChanged != null) OnHeartsChanged(hearts);
    }

    public void SetShield(float duration)
    {
        shieldTimer = duration;
    }
}
