using UnityEngine;

// The escape portal: advances toward the player, spins, triggers Victory on contact.
public class PortalController : MonoBehaviour
{
    public float spinSpeed = 90f; // degrees per second

    void Update()
    {
        if (GameManager.I == null || GameManager.I.state != GameManager.State.Playing) return;
        transform.position += Vector3.back * GameManager.I.ForwardSpeed * Time.deltaTime;
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHealth>() == null) return;
        if (GameManager.I.state != GameManager.State.Playing) return;

        GameManager.I.state = GameManager.State.Victory;
        if (ScreenFlash.I != null) ScreenFlash.I.Flash(Color.white, 0.9f, 0.8f); // light burst
        if (VictoryScreen.I != null) VictoryScreen.I.Show();
    }
}
