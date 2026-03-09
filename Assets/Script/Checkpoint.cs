using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color defaultColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) defaultColor = sr.color;
    }

    // ✅ ลบ activated flag ออก — ให้ Overlap ทุกครั้งอัปเดต Checkpoint ล่าสุดเสมอ
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CheckpointManager.Instance.SetCheckpoint(transform.position);

        if (sr != null) sr.color = Color.yellow;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // ✅ คืนสีเดิมเมื่อออกจาก Checkpoint
        if (sr != null) sr.color = defaultColor;
    }
}