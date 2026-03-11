using UnityEngine;
using System.Collections;

public class RandomBulletSpawner : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;

    [Header("Spawn Area")]
    public Vector2 spawnAreaSize = new Vector2(1f, 10f);  // ขนาดพื้นที่สุ่ม (กว้าง × สูง)
    public Vector2 spawnAreaOffset = Vector2.zero;        // ตำแหน่ง Offset จาก Spawner

    [Header("Bullet Count")]
    public int bulletCount = 10;  // จำนวนกระสุนที่จะยิง

    [Header("Timing")]
    public float delayBeforeStart = 0f;     // Delay ก่อนเริ่มยิง
    public float delayBetweenBullets = 1f;// Delay ระหว่างแต่ละนัด

    [Header("Bullet Movement")]
    public Vector2 bulletDirection = Vector2.left;  // ทิศทางกระสุน
    public float bulletSpeed = 10f;
    public float bulletLifetime = 5f;

    [Header("Options")]
    public bool canTriggerMultipleTimes = false;
    public bool resetOnPlayerDeath = true;

    private bool hasTriggered = false;
    private Coroutine spawnCoroutine;

    void Start()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab ยังไม่ได้กำหนด!");
        }

        if (resetOnPlayerDeath && GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.AddListener(ResetSpawner);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasTriggered || canTriggerMultipleTimes)
            {
                StartShooting();
            }
        }
    }

    void StartShooting()
    {
        hasTriggered = true;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        spawnCoroutine = StartCoroutine(SpawnBullets());
    }

    IEnumerator SpawnBullets()
    {
        // 1. รอก่อนเริ่มยิง
        if (delayBeforeStart > 0)
        {
            yield return new WaitForSeconds(delayBeforeStart);
        }

        Debug.Log($"เริ่มยิงกระสุน {bulletCount} นัด!");

        // 2. ยิงกระสุนทีละนัด
        for (int i = 0; i < bulletCount; i++)
        {
            SpawnBulletAtRandomPosition();

            // 3. รอก่อนยิงนัดถัดไป
            if (i < bulletCount - 1)
            {
                yield return new WaitForSeconds(delayBetweenBullets);
            }
        }

        Debug.Log("ยิงครบแล้ว!");
        spawnCoroutine = null;
    }

    void SpawnBulletAtRandomPosition()
    {
        if (bulletPrefab == null) return;

        // คำนวณตำแหน่งสุ่มในพื้นที่ที่กำหนด
        Vector2 randomPosition = GetRandomSpawnPosition();

        // สร้างกระสุน
        GameObject bullet = Instantiate(bulletPrefab, randomPosition, Quaternion.identity);

        // ตั้งค่ากระสุน
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript == null)
        {
            bulletScript = bullet.AddComponent<Bullet>();
        }

        bulletScript.Initialize(bulletDirection.normalized, bulletSpeed, bulletLifetime);
    }

    Vector2 GetRandomSpawnPosition()
    {
        // ตำแหน่งกลางของพื้นที่
        Vector2 center = (Vector2)transform.position + spawnAreaOffset;

        // สุ่มตำแหน่งภายในพื้นที่
        float randomX = Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f);
        float randomY = Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f);

        return center + new Vector2(randomX, randomY);
    }

    public void ResetSpawner()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        hasTriggered = false;

        Debug.Log("RandomBulletSpawner Reset");
    }

    void OnDestroy()
    {
        if (resetOnPlayerDeath && GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerDie.RemoveListener(ResetSpawner);
        }
    }

    // ✅ แสดงพื้นที่สุ่มใน Editor
    void OnDrawGizmosSelected()
    {
        Vector2 center = (Vector2)transform.position + spawnAreaOffset;

        // วาดกรอบพื้นที่สุ่ม
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(center, spawnAreaSize);

        // วาดทิศทางกระสุน
        Gizmos.color = Color.red;
        Vector2 direction = bulletDirection.normalized * 2f;
        Gizmos.DrawRay(center, direction);
        Gizmos.DrawSphere(center + direction, 0.2f);
    }
}