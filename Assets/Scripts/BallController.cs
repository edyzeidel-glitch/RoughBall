using UnityEngine;

// Moves the ball around the inner wall of the tube using pure math (no physics).
// A = rotate left, D = rotate right. Snappy with a small acceleration curve.
public class BallController : MonoBehaviour
{
    [Header("Tube")]
    public float tubeRadius = 5f;
    public float ballRadius = 0.4f;

    [Header("Feel")]
    public float rotateSpeed = 200f;   // max degrees per second
    public float snappiness = 10f;     // how fast we reach max speed (higher = snappier)

    [Header("State")]
    public float angle = -90f;         // -90 = bottom of tube

    private float angVel;              // current angular velocity

void Update()
    {
        if (GameManager.I != null && GameManager.I.state != GameManager.State.Playing) return;

        // New Input System (project is set to Input System only)
        float input = 0f;
        var kb = UnityEngine.InputSystem.Keyboard.current;
        if (kb != null)
        {
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input += 1f;
        }

        // ease toward target speed = snappy but smooth
        float target = input * rotateSpeed;
        angVel = Mathf.MoveTowards(angVel, target, snappiness * rotateSpeed * Time.deltaTime);
        angle += angVel * Time.deltaTime;

        float rad = angle * Mathf.Deg2Rad;
        float r = tubeRadius - ballRadius;
        transform.position = new Vector3(Mathf.Cos(rad) * r, Mathf.Sin(rad) * r, 0f);

        // visual roll: forward spin + side spin when steering
        transform.Rotate(360f * Time.deltaTime, 0f, -angVel * 0.5f * Time.deltaTime, Space.Self);
    }
}
