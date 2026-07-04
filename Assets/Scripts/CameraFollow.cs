using UnityEngine;

// Follows the ball around the tube wall with a smooth lag.
// Sits behind + above the ball (toward tube center), always looking forward.
public class CameraFollow : MonoBehaviour
{
    public BallController ball;

    [Header("Position")]
    public float heightAboveBall = 2.2f;  // toward tube center
    public float distanceBehind = 5f;
    public float lookAhead = 12f;

    [Header("Feel")]
    public float followSpeed = 8f;        // higher = tighter follow

    private float camAngle;

    void Start()
    {
        if (ball != null) camAngle = ball.angle;
    }

    void LateUpdate()
    {
        if (ball == null) return;

        // smooth lag: camera angle chases ball angle
        camAngle = Mathf.LerpAngle(camAngle, ball.angle, 1f - Mathf.Exp(-followSpeed * Time.deltaTime));

        float rad = camAngle * Mathf.Deg2Rad;
        Vector3 radial = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        float ballR = ball.tubeRadius - ball.ballRadius;

        Vector3 pos = radial * (ballR - heightAboveBall) + Vector3.back * distanceBehind;
        transform.position = pos;

        // look ahead into the tube; "up" points toward tube center so ball stays at screen bottom
        Vector3 lookTarget = radial * (ballR - heightAboveBall * 0.6f) + Vector3.forward * lookAhead;
        transform.rotation = Quaternion.LookRotation(lookTarget - pos, -radial);
    }
}
