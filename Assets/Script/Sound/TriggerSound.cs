using UnityEngine;

public class TriggerSound : MonoBehaviour
{
    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // ✅ เชื่อมต่อกับ GameManager: เมื่อเกิดใหม่ ให้เรียก ResetTrigger
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerRespawn.AddListener(ResetTrigger);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เช็ค Tag และต้องยังไม่เคยเล่น
        if (other.CompareTag("Player") && !hasPlayed)
        {
            if (audioSource != null)
            {
                audioSource.Play();
                hasPlayed = true; // ล็อคไว้
                Debug.Log("Sound played! (Locked until respawn)");
            }
        }
    }

    public void ResetTrigger()
    {
        hasPlayed = false; // ปลดล็อคให้เล่นได้อีกครั้ง
        Debug.Log("Trigger Sound Reset");
    }

    private void OnDestroy()
    {
        // คืนหน่วยความจำเมื่อ Object ถูกทำลาย
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerRespawn.RemoveListener(ResetTrigger);
        }
    }
}