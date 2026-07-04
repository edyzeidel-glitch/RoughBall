using UnityEngine;

// Rotates a barrier group around the tube axis while it advances (levels 4+).
public class BarrierRotator : MonoBehaviour
{
    public float rotSpeed = 8f; // degrees per second

    void Update()
    {
        if (GameManager.I == null || GameManager.I.state != GameManager.State.Playing) return;
        transform.Rotate(0f, 0f, rotSpeed * Time.deltaTime);
    }
}
