using UnityEngine;

// Spawns the escape portal once when the final stretch begins.
public class PortalSpawner : MonoBehaviour
{
    public GameObject portalPrefab;

    private bool spawned;

    void Update()
    {
        var gm = GameManager.I;
        if (gm == null || spawned || !gm.finalStretch || gm.state != GameManager.State.Playing) return;

        spawned = true;
        Instantiate(portalPrefab, new Vector3(0f, 0f, 110f), Quaternion.identity);
    }
}
