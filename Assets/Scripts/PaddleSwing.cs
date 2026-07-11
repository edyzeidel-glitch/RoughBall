using UnityEngine;

// Gives paddles a little Pong-swat: when the ball comes close, the paddle
// pulls back briefly (anticipation) then snaps toward the ball's side.
public class PaddleSwing : MonoBehaviour
{
    [Header("Trigger")]
    public float triggerDistance = 12f;   // how close (in z) the ball must be
    public float cooldown = 1.5f;         // seconds between swings
    public float lateralRange = 4.5f;     // how close sideways the ball must be (only nearby segments react)
    public bool jabOnly = false;          // wall segments: forward jab only, no sideways slide
    public float jabAmount = 0.8f;        // forward push toward the player
    public float wallJabBoost = 1.6f;     // wall segments jab this much harder than lone paddles



    [Header("Feel")]
    public float windup = 0.35f;          // pull-back distance (world units)
    public float lunge = 0.9f;            // snap distance toward the ball
    public float windupTime = 0.18f;
    public float lungeTime = 0.10f;
    public float settleTime = 0.25f;

    private Transform ball;
    private Vector3 basePos;
    private float timer;      // phase clock
    private int phase;        // 0 idle, 1 windup, 2 lunge, 3 settle
    private float dir;        // +1 or -1 along our tangent, toward the ball
    private float cdLeft;

    void Start()
    {
        var b = GameObject.Find("Ball");
        if (b != null) ball = b.transform;
        basePos = transform.localPosition;
    }

void Update()
    {
        if (GameManager.I == null || GameManager.I.state != GameManager.State.Playing) return;
        if (ball == null) return;

        if (phase == 0) basePos = transform.localPosition;
        cdLeft -= Time.deltaTime;

        if (phase == 0)
        {
            float dz = transform.position.z - ball.position.z;
            Vector2 lat = new Vector2(transform.position.x - ball.position.x, transform.position.y - ball.position.y);
            if (cdLeft <= 0f && dz > 0f && dz < triggerDistance && lat.magnitude < lateralRange)
            {
                Vector3 toBall = ball.position - transform.position;
                dir = Mathf.Sign(Vector3.Dot(toBall, transform.right));
                phase = 1; timer = 0f;
            }
            return;
        }

        timer += Time.deltaTime;
        float swat = 0f, jab = 0f;

        if (phase == 1) // anticipation: pull back and slightly away
        {
            float t = Mathf.Clamp01(timer / windupTime);
            float s = Mathf.Sin(t * Mathf.PI * 0.5f);
            swat = -dir * windup * s;
            jab = -0.2f * jabAmount * s;
            if (t >= 1f) { phase = 2; timer = 0f; }
        }
        else if (phase == 2) // strike: snap sideways + push forward
        {
            float t = Mathf.Clamp01(timer / lungeTime);
            swat = Mathf.Lerp(-dir * windup, dir * lunge, t);
            jab = Mathf.Lerp(-0.2f * jabAmount, jabAmount, t);
            if (t >= 1f) { phase = 3; timer = 0f; }
        }
        else // settle home
        {
            float t = Mathf.Clamp01(timer / settleTime);
            swat = dir * lunge * (1f - t);
            jab = jabAmount * (1f - t);
            if (t >= 1f) { phase = 0; cdLeft = cooldown; transform.localPosition = basePos; return; }
        }

        if (jabOnly) { swat = 0f; jab *= wallJabBoost; } // wall segments: no sideways slide, harder jab
        Vector3 tangent = transform.localRotation * Vector3.right;
        transform.localPosition = basePos + tangent * swat + Vector3.back * jab;
    }
}
