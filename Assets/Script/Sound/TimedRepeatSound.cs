using UnityEngine;
using System.Collections;

public class TimedLoopOncePerLife : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasTriggeredInThisLife = false; // เช็คว่าในชีวิตนี้เล่นไปหรือยัง
    private Coroutine soundRoutine;

    [Header("Settings")]
    public float durationLimit = 5.0f; // จะให้เล่นวนไปนานกี่วินาที?
    public float repeatRate = 0.5f;    // ระยะห่างระหว่างแต่ละครั้งที่ดัง (ความถี่)

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // เชื่อมต่อกับ GameManager: เมื่อเกิดใหม่ ให้รีเซ็ตสถานะเพื่อให้เล่นได้อีกครั้ง
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerRespawn.AddListener(ResetForNewLife);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เงื่อนไข: ต้องเป็น Player และ "ยังไม่เคยเล่นเลยในชีวิตนี้"
        if (other.CompareTag("Player") && !hasTriggeredInThisLife)
        {
            hasTriggeredInThisLife = true; // ล็อคไว้ทันทีว่าเล่นแล้ว
            if (soundRoutine != null) StopCoroutine(soundRoutine);
            soundRoutine = StartCoroutine(PlaySoundForDuration());
        }
    }

    IEnumerator PlaySoundForDuration()
    {
        float timer = 0f;
        Debug.Log($"เริ่มเล่นเสียงวน {durationLimit} วินาที (เล่นครั้งเดียวในชีวิตนี้)");

        while (timer < durationLimit)
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            yield return new WaitForSeconds(repeatRate);
            timer += repeatRate;
        }

        Debug.Log("จบการเล่นเสียงตามเวลาที่กำหนด");
        soundRoutine = null;
    }

    // ฟังก์ชันนี้จะถูกเรียกโดย GameManager เมื่อกด Respawn
    public void ResetForNewLife()
    {
        // 1. หยุด Coroutine ที่กำลังทำงานอยู่ (ถ้ามี)
        if (soundRoutine != null)
        {
            StopCoroutine(soundRoutine);
            soundRoutine = null;
        }

        // 2. หยุดเสียงที่ค้างอยู่
        if (audioSource != null) audioSource.Stop();

        // 3. ปลดล็อคให้เดินชนแล้วดังใหม่ได้
        hasTriggeredInThisLife = false;

        Debug.Log("Reset เสียง: พร้อมสำหรับการชนรอบใหม่ในชีวิตนี้");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerRespawn.RemoveListener(ResetForNewLife);
        }
    }
}