using UnityEngine;

// Scrolls the tube grid texture toward the player to fake forward motion.
public class TubeScroller : MonoBehaviour
{
    public float scrollSpeed = 2f; // grid cells per second, raised later by GameManager

    private Renderer rend;
    private float offset;

    void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (GameManager.I != null && GameManager.I.state != GameManager.State.Playing) return; // world frozen in menus
        
offset += scrollSpeed * Time.deltaTime;
        rend.material.SetTextureOffset("_BaseMap", new Vector2(0f, -offset));
    }
}
