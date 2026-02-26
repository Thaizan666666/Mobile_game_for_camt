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
    public float deathDelayTime = 2f;

    public UnityEvent OnPlayerDie = new UnityEvent();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    public void PlayerDied(Vector3 deathPosition)
    {
        Debug.Log("Player Died - Starting Death Sequence");

        // เรียก Event สำหรับ Reset Objects
        OnPlayerDie?.Invoke();

        // เริ่ม Coroutine สำหรับ Death Sequence
        StartCoroutine(DeathSequence(deathPosition));
    }

    IEnumerator DeathSequence(Vector3 position)
    {
        // ✅ สร้าง Particle Effect ที่ตำแหน่งที่ตาย
        if (deathParticlePrefab != null)
        {
            GameObject particle = Instantiate(deathParticlePrefab, position, Quaternion.identity);
            Destroy(particle, 5f);
        }

        // ✅ ใช้ WaitForSecondsRealtime แทน WaitForSeconds
        yield return new WaitForSecondsRealtime(deathDelayTime);

        // ✅ แสดง Death UI
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;  // หยุดเกม
            Debug.Log("ACTIVE");
        }
    }

    public void RestartGame()
    {
        // ✅ ซ่อน Death Panel ก่อน Restart
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("InGame");
    }

    public void LoadMainMenu()
    {
        // ✅ ซ่อน Death Panel ก่อนกลับ Menu
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}