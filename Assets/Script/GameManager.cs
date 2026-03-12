using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject deathPanel;

    [Header("Death Effect")]
    public GameObject deathParticlePrefab;
    public float deathDelayTime = 1f; // ✅ Delay 1 วิก่อนขึ้น UI

    public UnityEvent OnPlayerDie = new UnityEvent();
    public UnityEvent OnPlayerRespawn = new UnityEvent();

    void Awake()
    {
        // ✅ ทุกครั้งที่โหลด Scene ให้สร้าง Instance ใหม่เสมอ
        if (Instance != null && Instance != this)
        {
            Destroy(Instance.gameObject); // ✅ ทำลาย Instance เก่า
            Instance = null;
        }

        Instance = this;

        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    public void PlayerDied(Vector3 deathPosition)
    {
        // ✅ หยุด Coroutine เก่าก่อนเสมอ ป้องกัน overlap
        StopAllCoroutines();

        OnPlayerDie?.Invoke();
        StartCoroutine(DeathSequence(deathPosition));
    }

    IEnumerator DeathSequence(Vector3 position)
    {
        // ✅ ซ่อน UI ก่อนเสมอ (reset สถานะ)
        if (deathPanel != null)
            deathPanel.SetActive(false);

        // ✅ Particle Effect
        if (deathParticlePrefab != null)
        {
            GameObject particle = Instantiate(deathParticlePrefab, position, Quaternion.identity);
            Destroy(particle, 5f);
        }

        // ✅ Delay 1 วิก่อนแสดง UI
        yield return new WaitForSecondsRealtime(deathDelayTime);

        // ✅ แสดง Death UI
        if (deathPanel != null)
            deathPanel.SetActive(true);
    }

    // ✅ เปลี่ยนจาก Restart → Respawn
    public void Respawn()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);

        foreach (var obj in FindObjectsByType<Respawnable>(FindObjectsSortMode.None))
            obj.ResetObject();

        OnPlayerRespawn?.Invoke();

        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
            player.Respawn();
    }

    public void LoadMainMenu()
    {
        SoundManager.StopAllLoops();
        if (deathPanel != null)
            deathPanel.SetActive(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    void OnDestroy()
    {
        // ✅ เคลียร์ Instance เมื่อถูกทำลาย
        if (Instance == this)
            Instance = null;
    }
}