using UnityEngine;

// Advances a paddle toward the player at the world's current speed.
public class PaddleMover : MonoBehaviour
{
    void Update()
    {
        if (GameManager.I == null || GameManager.I.state != GameManager.State.Playing) return;

        transform.position += Vector3.back * GameManager.I.ForwardSpeed * Time.deltaTime;

        if (transform.position.z < -15f)
            Destroy(gameObject);
    }
}
