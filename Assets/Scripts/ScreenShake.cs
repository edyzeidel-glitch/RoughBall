using UnityEngine;

// Camera shake on hit. Runs after CameraFollow (execution order 100)
// so the shake offset stacks on top of the follow position.
[DefaultExecutionOrder(100)]
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake I;

    private float timer, duration, magnitude;

    void Awake()
    {
        I = this;
    }

    public void Shake(float dur, float mag)
    {
        timer = duration = dur;
        magnitude = mag;
    }

    void LateUpdate()
    {
        if (timer <= 0f) return;
        timer -= Time.deltaTime;
        float falloff = timer / duration; // shake fades out
        transform.position += (Vector3)(Random.insideUnitCircle * magnitude * falloff);
    }
}
