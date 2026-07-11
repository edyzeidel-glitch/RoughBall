using UnityEngine;

// Central game clock: tracks time, levels (30s each, 5 total), and world speed.
// Other systems read ForwardSpeed and listen to OnLevelUp.
public class GameManager : MonoBehaviour
{
    public static GameManager I; // easy global access

    public enum State { Menu, Playing, GameOver, Victory, Paused }

    [Header("State")]
    public State state = State.Menu; // game waits on the start menu
    public int level = 1;
    public float elapsed;
    public bool finalStretch; // true after last level -> portal time

    [Header("Difficulty")]
    public float levelDuration = 30f;
    public int maxLevel = 5;
    public float baseSpeed = 9f;       // world units per second at level 1
    public float speedPerLevel = 1.75f; // added speed per level (L5 tops out ~same as before)

    [Header("References")]
    public TubeScroller tube;

    public System.Action<int> OnLevelUp;

    // current forward speed of the world rushing at the player
    public float ForwardSpeed { get { return baseSpeed + speedPerLevel * (level - 1); } }

    void Awake()
    {
        I = this;
    }

    void Update()
    {
        if (state != State.Playing) return;

        elapsed += Time.deltaTime;

        int targetLevel = Mathf.Min(maxLevel, 1 + (int)(elapsed / levelDuration));
        if (targetLevel > level)
        {
            level = targetLevel;
            if (OnLevelUp != null) OnLevelUp(level);
        }

        if (!finalStretch && elapsed >= levelDuration * maxLevel)
            finalStretch = true;

        // grid cell = 2 world units, so scroll speed in cells = speed / 2
        if (tube != null) tube.scrollSpeed = ForwardSpeed / 2f;
    }
}
