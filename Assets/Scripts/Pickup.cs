using UnityEngine;

// Collectible floating in the tube. Heart = +1 life, Shield = 3s invincibility.
// Pulses while alive; collected when the ball rolls through it.
public class Pickup : MonoBehaviour
{
    public enum Kind { Heart, Shield }
    public Kind kind = Kind.Heart;
    public float shieldDuration = 3f;
    public float pulseSpeed = 4f;
    public float pulseAmount = 0.15f;

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        // pulsing glow animation
        float s = 1f + pulseAmount * Mathf.Sin(Time.time * pulseSpeed);
        transform.localScale = baseScale * s;
        if (kind == Kind.Shield)
            transform.Rotate(0f, 90f * Time.deltaTime, 45f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        var hp = other.GetComponent<PlayerHealth>();
        if (hp == null) return;

        if (kind == Kind.Heart)
        {
            hp.Heal(1);
            if (AudioManager.I != null) AudioManager.I.PlayPickup();
            if (PickupPopup.I != null) PickupPopup.I.Show("+1 HEART!", new Color(1f, 0.3f, 0.45f));

        }
        else
        {
            hp.SetShield(shieldDuration);
            if (AudioManager.I != null) AudioManager.I.PlayShield();
            if (PickupPopup.I != null) PickupPopup.I.Show("IMMUNITY!", new Color(1f, 0.85f, 0.2f));

        }

        if (ScreenFlash.I != null)
        {
            Color c = kind == Kind.Heart ? new Color(1f, 0.2f, 0.4f) : new Color(1f, 0.85f, 0.2f);
            ScreenFlash.I.Flash(c, 0.2f, 0.25f);
        }
        Destroy(gameObject);
    }
}
