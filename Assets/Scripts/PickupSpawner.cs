using UnityEngine;
using System.Collections;


// Spawns heart/shield pickups on the ball's path every few seconds.
public class PickupSpawner : MonoBehaviour
{
    public GameObject heartPrefab;
    public GameObject shieldPrefab;

    public float spawnZ = 100f;
    public float minInterval = 5f;
    public float maxInterval = 8f;
    public float pathRadius = 4.55f;      // same radius the ball rides at
    public float heartChance = 0.6f; // slightly more shields

    private float timer = 4f;

    void Update()
    {
        var gm = GameManager.I;
        if (gm == null || gm.state != GameManager.State.Playing || gm.finalStretch) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = Random.Range(minInterval, maxInterval);
            float ang = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f) * pathRadius + Vector3.forward * spawnZ;
            var prefab = Random.value < heartChance ? heartPrefab : shieldPrefab;
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }


// Guaranteed care package at the start of hard levels (4 and 5).
    void Start()
    {
        if (GameManager.I != null) GameManager.I.OnLevelUp += OnLevelUp;
    }

    void OnLevelUp(int lvl)
    {
        if (lvl >= 4) StartCoroutine(LevelBonus());
    }

    IEnumerator LevelBonus()
    {
        yield return new WaitForSeconds(1.5f);
        SpawnOne(heartPrefab);
        yield return new WaitForSeconds(4f);
        SpawnOne(shieldPrefab);
    }

    void SpawnOne(GameObject prefab)
    {
        if (GameManager.I == null || GameManager.I.state != GameManager.State.Playing) return;
        float ang = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 pos = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0f) * pathRadius + Vector3.forward * spawnZ;
        Instantiate(prefab, pos, Quaternion.identity);
    }
}
