using UnityEngine;

// Spawns single paddles and barrier walls (ring with one gap).
// Gap shrinks per level; barriers rotate at level 4+.
public class PaddleSpawner : MonoBehaviour
{
    public GameObject paddlePrefab;

    [Header("Level Colors")] // emission color per level, contrasting neon
    public Color[] levelColors = new Color[] {
        new Color(2.2f, 0.15f, 1.3f),  // L1 magenta
        new Color(2.4f, 1.2f, 0.1f),   // L2 orange
        new Color(0.1f, 2.2f, 2.0f),   // L3 cyan
        new Color(1.6f, 0.2f, 2.4f),   // L4 purple
        new Color(2.6f, 0.15f, 0.25f)  // L5 red
    };


    [Header("Spawning")]
    public float spawnZ = 100f;
    public float baseInterval = 1.9f;
    public float intervalPerLevel = 0.35f; // steeper ramp: mid levels press harder
    public float minInterval = 1.1f;
    [Header("Intro (Pong-style opening)")]
    public float introDuration = 20f;     // singles-only warm-up at run start
    public float introPaddleWidth = 3f;   // narrow Pong-like bars (normal = 4.8)

    [Header("Barriers")]
    public float gapBase = 80f;       // gap size in degrees at level 1
    public float gapPerLevel = 13f;   // shrink per level (faster squeeze in mid levels)
    public float gapMin = 38f;
    public float barrierChanceBase = 0.35f; // walls are the real threat - more of them early
    public float barrierChancePerLevel = 0.08f;

    [Header("Tube")]
    public float paddleRadius = 4.25f;

    private float timer = 1.5f;

    void Update()
    {
        var gm = GameManager.I;
        if (gm == null || gm.state != GameManager.State.Playing || gm.finalStretch) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            float interval = Mathf.Max(minInterval, baseInterval - intervalPerLevel * (gm.level - 1));
            float barrierChance = barrierChanceBase + barrierChancePerLevel * gm.level;

            // Pong-style intro: only lone narrow paddles at the start of a run
            if (gm.elapsed < introDuration)
            {
                var intro = SpawnPaddle(Random.Range(0f, 360f));
                Vector3 s = intro.transform.localScale;
                intro.transform.localScale = new Vector3(introPaddleWidth, s.y, s.z);
                timer = interval;
                return;
            }

            if (Random.value < barrierChance)
            {
                float gap = Mathf.Max(gapMin, gapBase - gapPerLevel * (gm.level - 1));
                float rot = 0f;
                if (gm.level >= 3) // rotation starts one level earlier, gently
                    rot = (Random.value < 0.5f ? -1f : 1f) * (6f + 4f * (gm.level - 3)); // L3 6, L4 10, L5 14 deg/s
                SpawnBarrier(Random.Range(0f, 360f), gap, rot);
                timer = interval * 1.6f; // barriers need more reaction room
            }
            else
            {
                float a1 = Random.Range(0f, 360f);
                SpawnPaddle(a1);
                // from level 2: half the time a second paddle comes with it
                if (gm.level >= 2 && Random.value < 0.5f)
                    SpawnPaddle(a1 + Random.Range(120f, 240f));
                timer = interval;
            }
        }
    }

    // One paddle on the wall at the given angle (degrees).
public GameObject SpawnPaddle(float angleDeg)
    {
        float rad = angleDeg * Mathf.Deg2Rad;
        Vector3 radial = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        Vector3 pos = radial * paddleRadius + Vector3.forward * spawnZ;
        var p = Instantiate(paddlePrefab, pos, Quaternion.Euler(0f, 0f, angleDeg - 90f));
        ApplyLevelColor(p);
        return p;
    }

    // Tint a paddle's emission to the current level color.
    void ApplyLevelColor(GameObject paddle)
    {
        if (GameManager.I == null || levelColors.Length == 0) return;
        int idx = Mathf.Clamp(GameManager.I.level - 1, 0, levelColors.Length - 1);
        var rend = paddle.GetComponent<Renderer>();
        if (rend != null) rend.material.SetColor("_EmissionColor", levelColors[idx]);
    }

    // Ring of paddles covering everything except one gap centered at gapCenterDeg.
public GameObject SpawnBarrier(float gapCenterDeg, float gapSizeDeg, float rotSpeed)
    {
        var parent = new GameObject("Barrier");
        parent.transform.position = new Vector3(0f, 0f, spawnZ);
        parent.AddComponent<PaddleMover>();
        if (rotSpeed != 0f)
        {
            var br = parent.AddComponent<BarrierRotator>();
            br.rotSpeed = rotSpeed;
        }

        float covered = 360f - gapSizeDeg;
        float paddleArc = 43f;  // one paddle's real visual arc (3.4 wide at radius 4.55)
        float segGap = 5f;      // slit between paddles — too thin for the ball
        float step = paddleArc + segGap;
        int count = Mathf.Max(1, Mathf.FloorToInt((covered + segGap) / step));
        float used = count * step - segGap;
        float gapEnd = gapCenterDeg + gapSizeDeg * 0.5f + (covered - used) * 0.5f;

        for (int i = 0; i < count; i++)
        {
            float ang = gapEnd + paddleArc * 0.5f + i * step;
            float rad = ang * Mathf.Deg2Rad;
            Vector3 radial = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            var p = Instantiate(paddlePrefab, parent.transform);
            p.transform.localPosition = radial * paddleRadius;
            p.transform.localRotation = Quaternion.Euler(0f, 0f, ang - 90f);
            Destroy(p.GetComponent<PaddleMover>()); // parent moves the whole wall
            var sw = p.GetComponent<PaddleSwing>();
            if (sw != null) sw.jabOnly = true; // segments jab forward only — walls stay sealed

            ApplyLevelColor(p);
        }
        return parent;
    }
}
